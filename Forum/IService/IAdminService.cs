using Forum.Models;
using Forum.ViewModels;

namespace Forum.IService
{
    public interface IAdminService
    {
        ICollection<ApplicationUser> Users();
        ICollection<Follow> Follows();
        ICollection<Friend> Friends();
        ICollection<FriendRequest> FriendRequests();
       ICollection<Post> Posts();
        ICollection<SavePost> SavePosts();
        ICollection<LikePost> LikePosts();
        ICollection<LikeComment> LikeComments();
        ICollection<LikeReplyToComment> likeReplyToComments();
        ICollection<UserReport> UserReports();
        ICollection<PostReport> PostReports();
        ICollection<Comment> Comments();
        ICollection<Chat> chats();
        ICollection<Message> Messages();
        ICollection<ReplyToComment> ReplyToComments();
        ICollection<BlockByAdmin> AdminBlocks();
        ICollection<BlockByUser> UserBlocks();



    }
}
