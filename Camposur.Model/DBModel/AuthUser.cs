using Microsoft.AspNet.Identity.EntityFramework;

namespace Camposur.Model.DBModel
{
    public class AuthUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public bool? AdminEnabled { get; set; }
    }
}
