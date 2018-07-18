using Camposur.Core;
using Camposur.Model.DBModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Camposur.DataAccess.IdentityExtensions
{
    public class GPSignInManager : SignInManager<AuthUser, string>
    {
        public GPSignInManager(GPUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AuthUser user)
        {
            return ((GPUserManager)UserManager).GenerateUserIdentityAsync(user);
        }

        public static GPSignInManager Create(IdentityFactoryOptions<GPSignInManager> options, IOwinContext context)
        {
            return new GPSignInManager(context.GetUserManager<GPUserManager>(), context.Authentication);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            // Verify if user is admin enabled
            var user = await UserManager.FindByEmailAsync(userName);
            if (user != null)
            {
                var userIsAdmin = await UserManager.IsInRoleAsync(user.Id, Constants.RoleNameAdmin);
                if (!userIsAdmin && (user.AdminEnabled != true) || !user.EmailConfirmed)
                    return SignInStatus.RequiresVerification;
            }

            return await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
        }
    }
}
