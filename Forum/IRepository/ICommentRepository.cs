using Forum.Models;

namespace Forum.IRepository
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetAll();
        Comment GetById(int id);
        Task Add(Comment comment);
        Task Update(Comment comment);
        Task Delete(int id);

        

    }
}
