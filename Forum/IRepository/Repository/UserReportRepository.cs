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


       


        public async Task Add(UserReport userReport)
        {
            _context.UserReports.Add(userReport);
            await _context.SaveChangesAsync();
        }


        public async Task Update(UserReport userReport)
        {
            _context.UserReports.Update(userReport);
            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            UserReport report = GetById(id);
            if (report != null)
            {
                _context.UserReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }


    }
}
