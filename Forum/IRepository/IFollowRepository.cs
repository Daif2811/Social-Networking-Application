using Forum.Models;

namespace Forum.IRepository
{
    public interface IFollowRepository
    {
        ICollection<Follow> GetAll();
        Follow GetById(int id);
        Follow GetByUserId(string userId, string currentUserId);
        bool CheckFollow (string userId, string currentUserId);
        ICollection<Follow> GetFollowerByUserId(string userId);
        ICollection<Follow> GetFollowedByUserId(string userId);
        Task Add (Follow follow);
        Task Update (Follow follow);
        Task Delete (int id);

    }





}
