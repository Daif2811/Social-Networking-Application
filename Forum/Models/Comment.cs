using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Forum.Models
{
    public class Comment : ParentForUsers
    {
        public int Id { get; set; }
        public int PostId { get; set; }




        public virtual Post Post { get; set; }
        public virtual ICollection<ReplyToComment> ReplyToComments { get; set; }
        public virtual ICollection<LikeComment> Likes { get; set; }


    }
}
