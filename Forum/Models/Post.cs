using System.Composition;

namespace Forum.Models
{
    public class Post : ParentForUsers
    {
        public int Id { get; set; }
       


        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikePost> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

    }
}
