using Camposur.Model.DBModel;
using System.ComponentModel.DataAnnotations;

namespace Camposur.Model.ViewModel
{
    public class AuthUserViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Description { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Role { get; set; }
        public bool? Enabled { get; set; }

        public AuthUserViewModel(){
        }

        public AuthUserViewModel(AuthUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Enabled = user.AdminEnabled;
            Description = user.Description;
        }

        public AuthUserViewModel(AuthUser user, string roleName = null)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Enabled = user.AdminEnabled;
            Role = roleName;
        }
    }
}
