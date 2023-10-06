using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class ParentForUsers
    {


        public string Content { get; set; }
        
        public string Image { get; set; }

        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public string UserId { get; set; }





        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }


        public virtual ApplicationUser User { get; set; }
    }
}
