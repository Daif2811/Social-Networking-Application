using Forum.Models;

namespace Forum.IRepository
{
    public interface IReplyToCommentRepository
    {
        ICollection<ReplyToComment> GetAll(int commentId);
        ReplyToComment GetById(int id);
        Task Add (ReplyToComment reply);
        Task Update (ReplyToComment reply);
        Task Delete (int id);



    }
}
