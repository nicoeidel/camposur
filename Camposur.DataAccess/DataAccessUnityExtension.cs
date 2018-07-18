using Camposur.DataAccess.IdentityExtensions;
using Camposur.Model.DBModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;

namespace Camposur.DataAccess
{
    public class DataAccessUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterInstance(new CamposurContext(), new PerResolveLifetimeManager());

            var contextInjectionConstructor = new InjectionConstructor(new ResolvedParameter<CamposurContext>());
            Container.RegisterType<IUserStore<AuthUser>, UserStore<AuthUser>>(contextInjectionConstructor);
            Container.RegisterType<RoleStore<IdentityRole>>(contextInjectionConstructor);

            Container.RegisterType<GPUserManager>();

            var roleStoreInjectionConstructor = new InjectionConstructor(new ResolvedParameter<RoleStore<IdentityRole>>());
            Container.RegisterType<RoleManager<IdentityRole>>(roleStoreInjectionConstructor);

            Container.RegisterType<GPSignInManager>();
        }
    }
}