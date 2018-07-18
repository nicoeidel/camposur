using Camposur.Model.DBModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Camposur.DataAccess.IdentityExtensions
{
    public class GPUserManager : UserManager<AuthUser>
    {
        public GPUserManager(CamposurContext context, IDataProtectionProvider protectionProvider, UserStore<AuthUser> userStore) : base(userStore)
        {
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            UserTokenProvider = new DataProtectorTokenProvider<AuthUser>(protectionProvider.Create("ASP.NET Identity"));

            UserValidator = new UserValidator<AuthUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AuthUser user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
