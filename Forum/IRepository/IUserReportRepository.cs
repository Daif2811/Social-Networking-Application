using Forum.Models;

namespace Forum.IRepository
{
    public interface IUserReportRepository
    {
        ICollection<UserReport> GetAllReports();
        ICollection<UserReport> GetAllReportsByUserId(string userId);
        UserReport GetById(int id);
        

        void Add(UserReport userReport);
        void Update(UserReport userReport);
        void Delete(int id);
    }
}
