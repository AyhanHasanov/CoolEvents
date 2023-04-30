using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoolEvents.ViewModels.Home
{
    public class RegisterVM
    {
        [DisplayName("Username: ")]
        [MinLength(4, ErrorMessage = "Username is too short!")]
        [MaxLength(64, ErrorMessage = "Username is too long!")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Username { get; set; }

        [DisplayName("Password: ")]
        [MinLength(4, ErrorMessage = "Password is too short!")]
        [MaxLength(64, ErrorMessage = "Password is too long!")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Password { get; set; }

        [DisplayName("First Name: ")]
        [MaxLength(64, ErrorMessage = "First name is too long!")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string FirstName { get; set; }

        [DisplayName("Last Name: ")]
        [MaxLength(64, ErrorMessage = "Last name is too long!")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string LastName { get; set; }
    }
}
