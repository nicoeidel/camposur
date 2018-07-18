using Camposur.Model.DBModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Camposur.DataAccess
{
    public class CamposurContext : IdentityDbContext<AuthUser>
    {
        public CamposurContext() : base("CamposurConnectionString", throwIfV1Schema: false)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderAddress> OrderAddresses { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customer { get; set; }

        public override int SaveChanges()
        {
            try
            {
                AddTimestamps();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var userName = HttpContext.Current?.User?.Identity.GetUserName();
            var userId = HttpContext.Current?.User?.Identity.GetUserId();

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).CreatedByName = userName;
                    ((BaseEntity)entity.Entity).CreatedById = userId;
                }

                ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
                ((BaseEntity)entity.Entity).ModifiedByName = userName;
                ((BaseEntity)entity.Entity).ModifiedById = userId;
            }
        }

    }
}
