namespace Camposur.DataAccess.Migrations
{
    using Camposur.DataAccess.IdentityExtensions;
    using Camposur.Model.DBModel;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.DataProtection;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CamposurContext>
    {
        // Add-Migration -Name "Initial" -ProjectName "Camposur.DataAccess" -StartUpProjectName "Camposur.DataAccess"

        // Update-Database -ProjectName "Camposur.DataAccess" -StartUpProjectName "Camposur.DataAccess"

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CamposurContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            EnsureRoles(context);
            EnsureSuperAdmin(context);

            context.SaveChanges();
        }

        private void EnsureRoles(CamposurContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Admin
            if (!roleManager.RoleExists(Core.Constants.RoleNameAdmin))
            {
                roleManager.Create(new IdentityRole(Core.Constants.RoleNameAdmin));
            }
        }

        private void EnsureSuperAdmin(CamposurContext context)
        {
            var userManager = new GPUserManager(context, new DpapiDataProtectionProvider("Default Provider"), new UserStore<AuthUser>(context));

            if (userManager.FindByEmail(Core.Constants.DefaultAdmin_Email) != null)
                return;

            // Create and save user
            AuthUser superAdmin = new AuthUser
            {
                Email = Core.Constants.DefaultAdmin_Email,
                EmailConfirmed = true,
                UserName = Core.Constants.DefaultAdmin_Email,
                FirstName = "Nicolás",
                LastName = "Eidelman",
                AdminEnabled = true
            };

            var result = userManager.Create(superAdmin, Core.Constants.DefaultAdmin_Password);

            // Get and assign admin role Role
            if (result.Succeeded)
            {
                var sa = userManager.FindByEmail(Core.Constants.DefaultAdmin_Email);
                userManager.AddToRole(sa.Id, Core.Constants.RoleNameAdmin);
            }
        }
    }
}
