using Forum.Models;

namespace Forum.IRepository
{
    public interface ISavePostRepository
    {
        ICollection<SavePost> GetAllByUserId(string currentUserId);
        SavePost GetById(int id);
        SavePost GetByPostIdAndUserId(int postId, string userId);
        bool CheckSave(int postId, string userId);
        Task Add (SavePost post);
        Task Delete (int id);
    }
}
