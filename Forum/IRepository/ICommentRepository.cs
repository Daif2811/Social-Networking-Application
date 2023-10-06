using Forum.Models;

namespace Forum.IRepository
{
    public interface ICommentRepository
    {
        Task<ICollection<Comment>> GetAll();
        Task<Comment> GetById(int id);
        Task Add(Comment comment);
        Task Update(Comment comment);
        Task Delete(int id);

        

    }
}
