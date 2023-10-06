using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class ReplyToCommentRepository : IReplyToCommentRepository
    {
        private readonly ForumContext _context;

        public ReplyToCommentRepository( ForumContext context)
        {
            _context = context;
        }
        public async Task<ICollection<ReplyToComment>> GetAll(int commentId)
        {
           var replies = await _context.ReplyToComments
                .Include(a => a.Comment).ThenInclude(a => a.User)
                .ToListAsync();
            return replies;
        }

        public async Task<ReplyToComment> GetById(int id)
        {
            var reply = await _context.ReplyToComments.Where(a => a.Id == id).Include(a => a.Comment).ThenInclude(a => a.User).SingleOrDefaultAsync();
            return reply;
        }


        public async Task Add(ReplyToComment reply)
        {
            _context.ReplyToComments.Add(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Update(ReplyToComment reply)
        {
            _context.ReplyToComments.Update(reply);
            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            ReplyToComment reply = await GetById(id);
            _context.ReplyToComments.Remove(reply);
            await _context.SaveChangesAsync();
        }


    }

}
