using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFriendRepository _friendRepository;

        public LikeController(ILikeRepository likeRepository, IPostRepository postRepository, ICommentRepository commentRepository, IReplyToCommentRepository replyToCommentRepository, UserManager<ApplicationUser> userManager, IFriendRepository friendRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _replyToCommentRepository = replyToCommentRepository;
            _userManager = userManager;
            _friendRepository = friendRepository;

        }


        // Get Current User
        public ApplicationUser CurrentUser()
        {
            //// Way to get UserId
            ////Claim userClaim = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier));
            ////string userId = userClaim.Value;


            // another way to get userId
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }




        // All Liked Posts
        public async Task<IActionResult> LikedPost()
        {
            var userId = CurrentUser().Id;

            var posts = await _likeRepository.GetAllLikedPost(userId);

            return View(posts);
        }





        // Like Post
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = CurrentUser().Id;

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





        // UnLike Post
        public async Task<IActionResult> UnlikePost(int postId)
        {

            var userId = CurrentUser().Id;

            Post post = await _postRepository.GetById(postId);
            post.LikeCount--;

            await _likeRepository.UnLikePost(postId, userId);

            return Json(new { success = true }); // Return a JSON response
        }




        public async Task<IActionResult> PostLikeUsers(int id)
        {
            var currentUserId = CurrentUser().Id;
            var likes = await _likeRepository.PostLikeUsers(id);
            foreach (var user in likes)
            {
                var friend = _friendRepository.CheckFriend(user.UserId, currentUserId);
                if (friend != null)
                {
                    ViewBag.IsFriend = true;
                }
                else
                {
                    ViewBag.IsFriend = false;

                }
            }

            return View(likes);
        }



        // Like Comment
        public async Task<IActionResult> LikeComment(int commentId)
        {
            var userId = CurrentUser().Id;

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






        // UnLike Comment
        public async Task<IActionResult> UnLikeComment(int commentId)
        {
            var userId = CurrentUser().Id;

            Comment comment = await _commentRepository.GetById(commentId);
            comment.LikeCount--;

            await _likeRepository.UnLikeComment(commentId, userId);

            return Json(new { success = true }); // Return a JSON response
        }




        public async Task<IActionResult> CommentLikeUsers(int id)
        {
            var likes = await _likeRepository.CommentLikeUsers(id);
            return View(likes);
        }


        // Like Reply To Comment
        public async Task<IActionResult> LikeReplyToComment(int replyId)
        {
            var userId = CurrentUser().Id;

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






        // UnLike Reply To Comment
        public async Task<IActionResult> UnLikeReplyToComment(int replyId)
        {
            var userId = CurrentUser().Id;

            ReplyToComment reply = await _replyToCommentRepository.GetById(replyId);
            reply.LikeCount--;

            await _likeRepository.UnLikeReplyToComment(replyId, userId);

            return Json(new { success = true }); // Return a JSON response
        }



        public async Task<IActionResult> ReplyLikeUsers(int id)
        {
            var likes = await _likeRepository.ReplyLikeUsers(id);
            return View(likes);
        }


    }
}
