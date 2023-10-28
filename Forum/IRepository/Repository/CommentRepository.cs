using Forum.DAL;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ForumContext _context;

        public CommentRepository(ForumContext context)
        {
            _context = context;
        }

        public ICollection<Comment> GetAll()
        {
            ICollection<Comment> comments = _context.Comments.Include(a => a.User).ToList();
            return comments;
        }

        public Comment GetById(int id)
        {
            Comment comment =  _context.Comments.Where(a => a.Id == id)
                .Include(a => a.User).Include(a => a.Likes)
                .Include(a => a.Post).ThenInclude(a => a.User)
                .Include(a => a.Likes).ThenInclude(a => a.User)
                .Include(a => a.ReplyToComments).ThenInclude(a => a.User )
                .Include(a => a.ReplyToComments).ThenInclude(a => a.Likes)
                .SingleOrDefault();
            return comment;
        }



        public async Task Add (Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Comment comment =  GetById(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }


       

    }
}
