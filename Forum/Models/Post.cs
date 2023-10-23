using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace Forum.Models
{

    public enum Audience
    {
        Public, Friends,
        
        OnlyMe
    }

    public class Post : ParentForUsers
    {
        public int Id { get; set; }
        public string Audience { get; set; }



        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikePost> Likes { get; set; }
        public virtual ICollection<SavePost> SavePosts { get; set; }
        public virtual ICollection<PostReport> PostReports { get; set; }

    }
}
