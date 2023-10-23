using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Forum.IRepository.Repository
{
    public class SavePostRepository : ISavePostRepository
    {
        private readonly ForumContext _context;
        public SavePostRepository(ForumContext context)
        {
            _context = context;
        }


        public ICollection<SavePost> GetAllByUserId(string currentUserId)
        {
            return _context.SavePosts.Where(a => a.UserId == currentUserId)
                .Include(a => a.Post).ThenInclude(a => a.User)
                .Include(a => a.Post).ThenInclude(a => a.Likes)
                .Include(a => a.Post).ThenInclude(a => a.Comments).ToList();
        }
        public SavePost GetById(int id)
        {
            return _context.SavePosts.Where(a => a.Id == id).SingleOrDefault();
        }
        public SavePost GetByPostIdAndUserId(int postId, string userId)
        {
            return _context.SavePosts.Where(a => a.PostId == postId && a.UserId == userId).SingleOrDefault();

        }

         public bool CheckSave(int postId, string userId)
        {
            return _context.SavePosts.Any(a => a.PostId == postId && a.UserId == userId);

        }


        public async Task Add(SavePost post)
        {
            _context.SavePosts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            SavePost post = GetById(id);
            if (post != null)
            {
                _context.SavePosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

    }
}
