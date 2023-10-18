using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public enum ReportText
    {
        FakeInformation, Bad, Violence, Stealing, Other, Porn 

    }

    public class Report
    {
        [Required]
        public ReportText Type { get; set; }

        [Required]
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
