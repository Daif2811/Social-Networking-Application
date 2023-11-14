using Forum.Models;

namespace Forum.ViewModels
{
    public class AdminViewModel
    {
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<LikePost> LikePosts { get; set; } = new List<LikePost>();
        public ICollection<BlockByAdmin> BlocksByAdmin { get; set; } = new List<BlockByAdmin>();
        public ICollection<BlockByUser> BlocksByUser { get; set; } = new List<BlockByUser>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Chat> Chats { get; set; } = new List<Chat>();
        public ICollection<PostReport> PostReports { get; set; } = new List<PostReport>();
        public ICollection<UserReport> UserReports { get; set; } = new List<UserReport>();

    }
}
