using Camposur.Model.DBModel;
using Camposur.Model.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camposur.BusinessLogic.Logic.Interfaces
{
    public interface IAuthLogic
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model, string redirectionLink);
        Task<SignInStatus> SignInUserAsync(LoginViewModel model, bool isPersistent);
        Task<SignInStatus> SignInUserAsync(RegisterViewModel model, bool isPersistent, bool rememberBrowser);
        bool IsUserInRole(string userId, string roleName);
        IEnumerable<AuthUserViewModel> GetUsersToEnable();
        Task<bool> ValidateEmailToken(string userId, string token);
        void SignOut();
        Task<ResetPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword, string actionUrl);
        Task<IdentityResult> ResetPassword(ResetPasswordViewModel resetPassword);
        bool ExistsUser(string email);
        AuthUser FindById(string userId);
        Task<bool> EnableUser(string userId, string roleId, string actionUrl);
        Task<bool> RejectUser(string userId);
        IEnumerable<IdentityRole> GetRoles();
        string GetRoleName(IdentityRole identityRole);
        IEnumerable<AuthUserViewModel> GetAllUsers();
        AuthUserViewModel GetAuthUser(string userId);
        string GetUserFriendlyName(string userId);
        AuthUser GetUser(string userId);
        void EditUser(AuthUser user);
        void DeleteUser(AuthUser user);
        IEnumerable<AuthUser> GetAuthUsers(bool onlyEnabled, params string[] roles);
    }
}
