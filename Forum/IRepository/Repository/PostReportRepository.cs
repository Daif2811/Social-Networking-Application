using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class PostReportRepository : IPostReportRepository
    {
        private readonly ForumContext _context;

        public PostReportRepository(ForumContext context)
        {
            _context = context;
        }



        public ICollection<PostReport> GetAllReports()
        {
           return _context.PostReports.Include(a => a.Post).ThenInclude(a => a.User).Include(a => a.Post.Comments).ThenInclude(a => a.User).Include(a => a.Post.Likes).ThenInclude(a => a.User).Include(a => a.Reporter).ToList();
        }

        public ICollection<PostReport> GetPostsByUserId(string userId)
        {
            return _context.PostReports.Where(b => b.Post.UserId == userId).Include(a => a.Post).ThenInclude(a => a.User).ToList();
        }

        public PostReport GetById(int id)
        {
            return _context.PostReports.Include(a => a.Post).ThenInclude(a => a.User).SingleOrDefault(b => b.Id == id);
        }



        public async Task Add (PostReport postReport)
        {
            _context.PostReports.Add(postReport);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PostReport postReport)
        {
            _context.PostReports.Update(postReport);
            await _context.SaveChangesAsync();
        }

        public async Task Delete (int id)
        {
            PostReport report = GetById(id);
            if (report != null)
            {
                _context.PostReports.Remove(report);
                await _context.SaveChangesAsync();
            }

        }
    }
}
