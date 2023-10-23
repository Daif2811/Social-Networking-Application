using Forum.Models;

namespace Forum.IRepository
{
    public interface IPostRepository
    {

        ICollection<Post> GetAll();
        ICollection<Post> Search(string searchName);
        ICollection<Post> Profile(string userId);
        Post GetById(int id);
        Task Add(Post post);
        Task Update( Post post);
        Task Delete(int id);


        Post GetAllComments(int id);


    }

}
