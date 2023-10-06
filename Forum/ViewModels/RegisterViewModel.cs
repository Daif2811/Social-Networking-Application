using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Forum.ViewModels
{
    public class RegisterViewModel
    {

        [Required, Display(Name = "User Name")]
        public string UserName { get; set; }


        [Display(Name = "Profile Picture")]
        public IFormFile Picture { get; set; }


        public string Summary { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }




        [Display(Name = "User Type")]
        public string UserType { get; set; }


         


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }




        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
