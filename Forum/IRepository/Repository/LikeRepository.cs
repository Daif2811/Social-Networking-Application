using Forum.DAL;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Forum.IRepository.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ForumContext _context;

        public LikeRepository(ForumContext context)
        {
            _context = context;
        }


        public async Task LikePost(LikePost like)
        {
            _context.LikePosts.Add(like);
            await _context.SaveChangesAsync();

        }


        public async Task UnLikePost(int postId, string userId)
        {
            var like = await _context.LikePosts.Where(a => a.PostId == postId && a.UserId == userId).FirstOrDefaultAsync();
            if (like != null)
            {
                _context.LikePosts.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        public ICollection<LikePost> GetAllLikedPost(string userId)
        {
            var posts = _context.LikePosts.Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Post).ThenInclude(a => a.User)
                .OrderByDescending(a => a.Id).ToList();

            return posts;
        }
         public ICollection<LikePost> PostLikeUsers(int id)
        {
            var posts =  _context.LikePosts.Where(a => a.PostId == id).Include(a => a.User)
                .Include(a => a.Post).ThenInclude(a => a.User)
                .ToList();
            return posts;
        }









        // Comment
        public async Task LikeComment(LikeComment like)
        {
                _context.LikeComments.Add(like);
                await _context.SaveChangesAsync();
        }


        public async Task UnLikeComment(int commentId, string userId)
        {
            var like = await _context.LikeComments.Where(a => a.CommentId == commentId && a.UserId == userId).FirstOrDefaultAsync();
            if (like != null)
            {
                _context.LikeComments.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        public ICollection<LikeComment> CommentLikeUsers(int id)
        {
            var comments =  _context.LikeComments.Where(a => a.CommentId == id).Include(a => a.User).ToList();
            return comments;
        }










        // Reply To Comment
        public async Task LikeReplyToComment(LikeReplyToComment like)
        {
            _context.LikeReplyToComments.Add(like);
            await _context.SaveChangesAsync();
        }


        public async Task UnLikeReplyToComment(int replyId, string userId)
        {
            var like = await _context.LikeReplyToComments.Where(a => a.ReplyId == replyId && a.UserId == userId).FirstOrDefaultAsync();
            if (like != null)
            {
                _context.LikeReplyToComments.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        public ICollection<LikeReplyToComment> ReplyLikeUsers(int id)
        {
            var reply =  _context.LikeReplyToComments.Where(a => a.ReplyId == id).Include(a => a.User).ToList();
            return reply;
        }

    }
}
