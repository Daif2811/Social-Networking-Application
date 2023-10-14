using Forum.Models;

namespace Forum.IRepository
{
    public interface ILikeRepository
    {
        // Post
        Task LikePost(LikePost like);
        Task UnLikePost(int postId, string userId);
        Task<ICollection<LikePost>> GetAllLikedPost(string userId);
        Task<ICollection<LikePost>> PostLikeUsers(int id);
        
        
        

        // Comment
        Task LikeComment(LikeComment like);
        Task UnLikeComment(int commentId, string userId);
        Task<ICollection<LikeComment>> CommentLikeUsers(int id);







        // Reply to Comment
        Task LikeReplyToComment(LikeReplyToComment like);
        Task UnLikeReplyToComment(int replyId, string userId);
        Task<ICollection<LikeReplyToComment>> ReplyLikeUsers(int id);










    }





}
