using Forum.Models;

namespace Forum.IRepository
{
    public interface IPostReportRepository
    {

        ICollection<PostReport> GetAllReports();
        ICollection<PostReport> GetPostsByUserId(string userId);
        PostReport GetById(int id);

        Task Add(PostReport postReport);
        Task Update(PostReport postReport);
        Task Delete(int id);
    }




    public interface IUserReportRepository
    {
        ICollection<UserReport> GetAllReports();
        ICollection<UserReport> GetAllReportsByUserId(string userId);
        UserReport GetById(int id);


        Task Add(UserReport userReport);
        Task Update(UserReport userReport);
        Task Delete(int id);
    }
}
