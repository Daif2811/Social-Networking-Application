using Forum.Models;

namespace Forum.IRepository
{
    public interface ILikeRepository
    {
        // Post
        Task LikePost(LikePost like);
        Task UnLikePost(int postId, string userId);
        Task<ICollection<LikePost>> GetAllLikedPost(string userId);
        
        
        

        // Comment
        Task LikeComment(LikeComment like);
        Task UnLikeComment(int commentId, string userId);







        // Reply to Comment
        Task LikeReplyToComment(LikeReplyToComment like);
        Task UnLikeReplyToComment(int replyId, string userId);







       

    }





}
