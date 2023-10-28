using Forum.Models;

namespace Forum.IRepository
{
    public interface IFollowRepository
    {
        ICollection<Follow> GetAll();
        Follow GetById(int id);
        Follow GetByUserId(string followedId, string followerId);
        bool CheckFollow (string followedId, string followerId);
        ICollection<Follow> GetFollowerByUserId(string followedId);
        ICollection<Follow> GetFollowedByUserId(string followerId);
        Task Add (Follow follow);
        Task Update (Follow follow);
        Task Delete (int id);

    }





}
