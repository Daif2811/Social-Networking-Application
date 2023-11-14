using Forum.DAL;
using Forum.Models;

namespace Forum.IService
{
    public class AdminService : IAdminService
    {
        private readonly ForumContext _context;

        public AdminService(ForumContext context)
        {
            _context = context;
        }
        public ICollection<ApplicationUser> Users() 
        { 
            return _context.Users.ToList();
        }

        public ICollection<Follow> Follows()
        {
            return _context.Follows.ToList();
        }

        public ICollection<Friend> Friends ()
        {
            return _context.Friends.ToList();
        }

        public ICollection<FriendRequest> FriendRequests()
        {
            return _context.FriendRequests.ToList();
        }

        public ICollection<Post> Posts()
        {
            return _context.Posts.ToList();
        }

        public ICollection<SavePost> SavePosts()
        {
            return _context.SavePosts.ToList();
        }

        public ICollection<LikePost> LikePosts()
        {
            return _context.LikePosts.ToList();
        }

        public ICollection<LikeComment> LikeComments()
        {
            return _context.LikeComments.ToList();
        }

        public ICollection<LikeReplyToComment> likeReplyToComments()
        {
            return _context.LikeReplyToComments.ToList();
        }

        public ICollection<UserReport> UserReports()
        {
            return _context.UserReports.ToList();
        }

        public ICollection<PostReport> PostReports()
        {
            return _context.PostReports.ToList();
        }

        public ICollection<Comment> Comments()
        {
            return _context.Comments.ToList();    
        }

        public ICollection<Chat> chats()
        {
            return _context.Chats.ToList();
        }

        public ICollection<Message> Messages ()
        {
            return _context.Messages.ToList();
        }

        public ICollection<ReplyToComment> ReplyToComments()
        {
            return _context.ReplyToComments.ToList();   
        }

        public ICollection<BlockByAdmin> AdminBlocks()
        {
            return _context.BlocksByAdmins.ToList();
        }

        public ICollection<BlockByUser> UserBlocks()
        {
            return _context.BlocksByUsers.ToList();
        }
    }
}
