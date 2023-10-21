using Forum.Models;

namespace Forum.IRepository
{
    public interface IPostReportRepository
    {

        ICollection<PostReport> GetAllReports();
        ICollection<PostReport> GetPostsByUserId(string userId);
        PostReport GetById(int id);

        void Add(PostReport postReport);
        void Update(PostReport postReport);
        void Delete(int id);
    }




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
