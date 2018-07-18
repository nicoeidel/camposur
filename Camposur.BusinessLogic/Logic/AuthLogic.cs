using Camposur.DataAccess.IdentityExtensions;
using Camposur.Model.DBModel;
using Camposur.Model.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Camposur.BusinessLogic.Logic.Interfaces
{
    public class AuthLogic : IAuthLogic
    {
        private readonly GPUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly GPSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISendGridLogic _sendGridLogic;

        public AuthLogic(GPUserManager userManager, RoleManager<IdentityRole> roleManager, GPSignInManager signInManager, IAuthenticationManager authenticationManager, ISendGridLogic sendGridLogic)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _sendGridLogic = sendGridLogic;
        }

        public bool ExistsUser(string email)
        {
            if (_userManager.FindByEmail(email) != null) return true;
            return false;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model, string redirectionLink)
        {
            var user = new AuthUser { FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, Email = model.Email, Description = model.Description };
            var result = await _userManager.CreateAsync(user, model.Password);

            // If user added then assign User role
            if (result.Succeeded)
            {
                var adminUsers = (await _roleManager.FindByNameAsync(Core.Constants.RoleNameAdmin))?.Users.ToList();

                if (adminUsers != null)
                {
                    foreach (var admin in adminUsers)
                    {
                        var adminUserId = admin.UserId;
                        var adminUser = await _userManager.FindByIdAsync(adminUserId);

                        await SendRegistrationAlertMail(user, adminUser.Email, redirectionLink);
                    }
                }
            }
            return result;
        }

        private async Task<bool> SendRegistrationAlertMail(AuthUser registered, string adminEmail, string redirectionLink)
        {
            if (registered == null)
                return false;

            await _sendGridLogic.SendUserRegistrationAlert(
                registered.Email,
                adminEmail,
                redirectionLink);

            return true;
        }

        private async Task<bool> SendEmailValidationToken(string email, string actionUrl)
        {
            AuthUser user = _userManager.FindByEmail(email);

            if (user == null)
                return false;

            var token = await GenerateEmailToken(user.Id);

            Uri callbackUrl = GenerateTokenUrl(actionUrl, Core.Constants.SendGridAuthLink_ConfirmEmail, user.Id, token);

            var result = await _sendGridLogic.SendEmailValidation(
                string.Format("{0} {1}", user.FirstName, user.LastName),
                user.Email,
                callbackUrl.ToString());

            return true;
        }

        public async Task<SignInStatus> SignInUserAsync(LoginViewModel model, bool isPersistent)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        }

        public async Task<SignInStatus> SignInUserAsync(RegisterViewModel model, bool isPersistent, bool rememberBrowser)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, rememberBrowser, shouldLockout: false);
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(roleName))
                return _userManager.IsInRole(userId, roleName);
            return false;
        }

        public AuthUser FindById(string userId)
        {
            return _userManager.FindById(userId);
        }

        public IEnumerable<AuthUserViewModel> GetUsersToEnable()
        {
            var adminRole = _roleManager.Roles.FirstOrDefault(r => r.Name == Core.Constants.RoleNameAdmin);
            if (adminRole == null)
                return null;

            var users = from user in _userManager.Users
                        where user.Roles.All(r => r.RoleId != adminRole.Id) && user.AdminEnabled == null
                        select user;
            return users.ToList().Select(u => new AuthUserViewModel(u)).ToList();
        }

        public async Task<ResetPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword, string actionUrl)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
                return new ResetPasswordResult(false, ResetPasswordError.InvalidEmail);

            try
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

                var url = GenerateTokenUrl(actionUrl, Core.Constants.SendGridAuthLink_ResetPassword, user.Id, resetToken);

                var result = await _sendGridLogic.SendPasswordReset(
                    $"{user.FirstName} {user.LastName}",
                    user.Email,
                    url.ToString());

                return new ResetPasswordResult(true);
            }
            catch (Exception e)
            {
                return new ResetPasswordResult(false, ResetPasswordError.Exception, e);
            }
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            return await _userManager.ResetPasswordAsync(resetPassword.UserId, resetPassword.Code, resetPassword.Password);
        }

        public void SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        private Uri GenerateTokenUrl(string actionUrl, string oauthLink, params string[] tokens)
        {
            Uri callbackUrl = new Uri(actionUrl);

            var paramList = new List<string>();
            paramList.AddRange(tokens.Select(HttpUtility.UrlEncode));

            var confirmUrl = string.Format(oauthLink, paramList.ToArray());

            callbackUrl = new Uri(callbackUrl, confirmUrl);

            return callbackUrl;
        }

        public async Task<string> GenerateEmailToken(string userId)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(userId);
        }

        public async Task<bool> ValidateEmailToken(string userId, string token)
        {
            var result = await ConfirmEmailToken(userId, token);
            return true;
        }

        public async Task<IdentityResult> ConfirmEmailToken(string userId, string token)
        {
            var user = await _userManager.ConfirmEmailAsync(userId, token);
            return user;
        }

        public async Task<bool> EnableUser(string userId, string roleId, string actionUrl)
        {
            var user = _userManager.FindById(userId);
            var role = _roleManager.FindById(roleId);

            // If user or role are wrong return false
            if (user == null || role == null)
                return false;

            user.AdminEnabled = true;

            IdentityResult result;

            result = await _userManager.UpdateAsync(user);

            // Update user roles
            var currentRoles = _userManager.GetRoles(user.Id);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user.Id, currentRoles.ToArray());

            await _userManager.AddToRoleAsync(user.Id, role.Name);

            await SendEmailValidationToken(user.Email, actionUrl);

            return result.Succeeded;
        }

        public async Task<bool> RejectUser(string userId)
        {
            var user = _userManager.FindById(userId);

            // If user is wrong return false
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public string GetRoleName(IdentityRole identityRole)
        {
            switch (identityRole.Name)
            {
                case Core.Constants.RoleNameAdmin:
                    //return Core.Resources.Roles_Admin;
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        public IEnumerable<AuthUserViewModel> GetAllUsers()
        {
            var admins = GetAuthUsers(Core.Constants.RoleNameAdmin).Select(u => new AuthUserViewModel(u, GetRoleFriendlyName(Core.Constants.RoleNameAdmin))).ToList();
            return admins;
        }

        public IEnumerable<AuthUser> GetAuthUsers(params string[] roles)
        {
            var rolesId = _roleManager.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id);
            var users = _userManager.Users.Where(user => user.AdminEnabled == true && user.Roles.Any(r => rolesId.Contains(r.RoleId)));

            return users.ToList();
        }

        public string GetRoleFriendlyName(string internalRoleName)
        {
            switch (internalRoleName)
            {
                case Core.Constants.RoleNameAdmin:
                    //return Resources.Roles_Admin;
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        public AuthUserViewModel GetAuthUser(string userId)
        {
            var user = GetUser(userId);
            var userViewModel = new AuthUserViewModel(user);

            return userViewModel;
        }

        public AuthUser GetUser(string userId)
        {
            return _userManager.FindById(userId);
        }

        public string GetUserFriendlyName(string userId)
        {
            var user = GetUser(userId);
            var userRoles = user?.Roles.Select(r => r.RoleId).ToList();

            if (userRoles == null || userRoles.Count == 0)
            {
                return "";
            }


            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == userRoles.FirstOrDefault());
            var roleName = GetRoleFriendlyName(role);
            return roleName;
        }

        public string GetRoleFriendlyName(IdentityRole role)
        {
            return GetRoleFriendlyName(role.Name);
        }

        public void EditUser(AuthUser user)
        {
            var userNoAdmin = _userManager.FindById(user.Id);

            userNoAdmin.FirstName = user.FirstName;
            userNoAdmin.LastName = user.LastName;
            userNoAdmin.Description = user.Description;

            _userManager.Update(userNoAdmin);
        }

        public void DeleteUser(AuthUser user)
        {
            _userManager.Delete(user);
        }

        public IEnumerable<AuthUser> GetAuthUsers(bool onlyEnabled, params string[] roles)
        {
            var rolesId = _roleManager.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id);
            var users = from user in _userManager.Users
                        where (!onlyEnabled || user.AdminEnabled == true) && user.Roles.Any(r => rolesId.Contains(r.RoleId))
                        select user;
            return users.ToList();
        }
    }
}
