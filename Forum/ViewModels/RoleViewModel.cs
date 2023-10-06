using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Forum.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
