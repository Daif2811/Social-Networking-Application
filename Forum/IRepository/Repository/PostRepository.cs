using Forum.DAL;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ForumContext _context;

        public PostRepository(ForumContext context )
        {
            _context = context;
        }

       
        public ICollection<Post> GetAll()
        {
            int pageSize = 10; // Adjust the page size as needed
            int pageNumber = 1; // The page number you want to retrieve

            var posts = _context.Posts
                .Include(a => a.User)
                .Include(a => a.Likes)
                .Include(a => a.Comments)
                .OrderByDescending(a => a.PublishDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            return posts;

        }

        public ICollection<Post> Search(string searchName)
        {
            return _context.Posts.Where
                (a => a.User.UserName.Contains(searchName)
                || a.Content.Contains(searchName)).
                Include(a => a.User).Include(a => a.Likes)
                .Include(a => a.Comments).
                OrderByDescending(a => a.PublishDate).ToList();

        }

        public ICollection<Post> Profile(string userId)
        {
            return _context.Posts.Where(a => a.UserId == userId).Include(a => a.User).Include(a => a.Likes).Include(a => a.Comments).OrderByDescending(a => a.PublishDate).ToList();

        }

        public Post GetById(int id)
        {
            Post post = _context.Posts.Include(a => a.Likes).Include(a => a.User).SingleOrDefault(a => a.Id == id);
            return post;
        }

        public Post GetAllComments(int id)
        {
            var post =  _context.Posts.Where(a => a.Id == id)
                .Include(a => a.Likes).Include(a => a.User)
                .Include(a => a.Comments).ThenInclude(a => a.User)
                .Include(a => a.Comments).ThenInclude(a => a.Likes)
                .Include(a => a.Comments).ThenInclude(a => a.ReplyToComments).ThenInclude(a => a.Likes)
                .Include(a => a.Comments).ThenInclude(a => a.ReplyToComments).ThenInclude(a => a.User)
                .SingleOrDefault();


            return post;
        }

        public async Task Add(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Update( Post post)
        {
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            
        }

        public async Task Delete(int id)
        {
            var post = GetById(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }


        //// Multiple updates
        //public async Task UpdateMany(IEnumerable<Post> posts)
        //{
        //    foreach (var post in posts)
        //    {
        //        _context.Posts.Update(post);
        //    }
        //    await _context.SaveChangesAsync();
        //}

    }
}
