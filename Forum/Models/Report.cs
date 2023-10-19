using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public enum ReportText
    {
        [Display(Name = "Fake Data")]
        FakeData,
        [Display(Name = "Bad")]
        Bad,
        [Display(Name = "Violence")]
        Violence,
        [Display(Name = "Stealing")] 
        Stealing,
        [Display(Name = "Porn")] 
        Porn,
        [Display(Name = "Other")] 
        Other

    }

    public class Report
    {
        [Required]
        public string Type { get; set; }

      
        public string ReporterId { get; set; }

        
        public virtual ApplicationUser Reporter { get; set; }
    }




    public class UserReport : Report
    {
        public int Id { get; set; }
       
        [Required]
        public string UserId { get; set; }



        public virtual ApplicationUser User { get; set; }
    } 
    
    
    public class PostReport : Report
    {
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }



        public virtual Post Post { get; set; }
    }
}
