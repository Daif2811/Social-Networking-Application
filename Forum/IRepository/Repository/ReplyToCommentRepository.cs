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
        public ICollection<ReplyToComment> GetAll(int commentId)
        {
           var replies =_context.ReplyToComments
                .Include(a => a.Comment).ThenInclude(a => a.User)
                .ToList();
            return replies;
        }

        public ReplyToComment GetById(int id)
        {
            var reply =  _context.ReplyToComments.Where(a => a.Id == id).Include(a => a.Comment).ThenInclude(a => a.User).SingleOrDefault();
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
            ReplyToComment reply = GetById(id);
            _context.ReplyToComments.Remove(reply);
            await _context.SaveChangesAsync();
        }


    }

}
