using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; }
        public string Picture { get; set; }
        public string Summary { get; set; }


        public virtual ICollection<Post> Posts { get; set; }
    }


  
}
