using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IReplyToCommentRepository _replyToCommentRepository;

        public LikeController(ILikeRepository likeRepository, IPostRepository postRepository, ICommentRepository commentRepository, IReplyToCommentRepository replyToCommentRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _replyToCommentRepository = replyToCommentRepository;
        }



        public async Task<IActionResult> LikedPost()
        {
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            var posts = await _likeRepository.GetAllLikedPost(userId);

            return View(posts);



        }






        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            // string userId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);




            LikePost like = new LikePost()
            {
                UserId = userId,
                PostId = postId,
            };
            
            Post post = await _postRepository.GetById(postId);
            post.LikeCount++;

            await _likeRepository.LikePost(like);

            return Json(new { success = true });

        }


        public async Task<IActionResult> UnlikePost(int postId)
        {
            // Get the current user's ID (you should implement user authentication)
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Post post = await _postRepository.GetById(postId);
            post.LikeCount--;

            // Remove the like from the repository
            await _likeRepository.UnLikePost(postId, userId);


            return Json(new { success = true }); // Return a JSON response
        }










        public async Task<IActionResult> LikeComment(int commentId)
        {
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            // string userId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);

            LikeComment like = new LikeComment()
            {
                UserId = userId,
                CommentId = commentId,
            };

            Comment comment = await _commentRepository.GetById(commentId);
            comment.LikeCount++;

            await _likeRepository.LikeComment(like);

            return Json(new { success = true });

        }


        public async Task<IActionResult> UnLikeComment(int commentId)
        {
            // Get the current user's ID (you should implement user authentication)
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           
            Comment comment = await _commentRepository.GetById(commentId);
            comment.LikeCount--;

            // Remove the like from the repository
            await _likeRepository.UnLikeComment(commentId, userId);


            return Json(new { success = true }); // Return a JSON response
        }








        public async Task<IActionResult> LikeReplyToComment(int replyId)
        {
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            // string userId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);

            LikeReplyToComment like = new LikeReplyToComment()
            {
                UserId = userId,
                ReplyId = replyId
            };

            ReplyToComment reply = await _replyToCommentRepository.GetById(replyId);
            reply.LikeCount++;

            await _likeRepository.LikeReplyToComment(like);

            return Json(new { success = true });

        }


        public async Task<IActionResult> UnLikeReplyToComment(int replyId)
        {
            // Get the current user's ID (you should implement user authentication)
            var userId = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            ReplyToComment reply = await _replyToCommentRepository.GetById(replyId);
            reply.LikeCount--;

            // Remove the like from the repository
            await _likeRepository.UnLikeReplyToComment(replyId, userId);


            return Json(new { success = true }); // Return a JSON response
        }














       

    }
}
