using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class UserReportRepository : IUserReportRepository
    {
        private readonly ForumContext _context;


        public UserReportRepository(ForumContext context)
        {
            _context = context;
        }
        public ICollection<UserReport> GetAllReports()
        {
            return _context.UserReports.Include(a => a.User).Include(a => a.Reporter).ToList();
        }

        public ICollection<UserReport> GetAllReportsByUserId(string userId)
        {
            return _context.UserReports.Where(a => a.UserId == userId).Include(a => a.User).ToList();
        }

        public UserReport GetById(int id)
        {
            return _context.UserReports.Where(a => a.Id == id).Include(a => a.User).FirstOrDefault();
        }


       


        public void Add(UserReport userReport)
        {
            _context.UserReports.Add(userReport);
            _context.SaveChanges();
        }


        public void Update(UserReport userReport)
        {
            _context.UserReports.Update(userReport);
            _context.SaveChanges();
        }


        public void Delete(int id)
        {
            UserReport report = GetById(id);
            if (report != null)
            {
                _context.UserReports.Remove(report);
                _context.SaveChanges();
            }
        }

    }
}
