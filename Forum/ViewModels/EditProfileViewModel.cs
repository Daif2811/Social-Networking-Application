using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Forum.ViewModels
{
    public class EditProfileViewModel
    {
        [Required, Display(Name = "User Name")]
        public string UserName { get; set; }



        [Required]
        [EmailAddress]
        public string Email { get; set; }



        [Display(Name = "Profile Picture")]
        public string Picture { get; set; }


        public string Summary { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }




        [Display(Name = "User Type")]
        public string UserType { get; set; }




        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


       



       


    }
}
