using Forum.Models;

namespace Forum.IRepository
{
    public interface IReplyToCommentRepository
    {
        Task<ICollection<ReplyToComment>> GetAll(int commentId);
        Task<ReplyToComment> GetById(int id);
        Task Add (ReplyToComment reply);
        Task Update (ReplyToComment reply);
        Task Delete (int id);



    }
}
