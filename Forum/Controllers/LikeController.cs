using Forum.IRepository;
using Forum.IRepository.Repository;
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
        private readonly IBlockByUserRepository _blockByUserRepository;
        private readonly ISavePostRepository _savePostRepository;

        public LikeController(
            ILikeRepository likeRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IReplyToCommentRepository replyToCommentRepository,
            UserManager<ApplicationUser> userManager,
            IFriendRepository friendRepository, 
            IBlockByUserRepository blockByUserRepository,
            ISavePostRepository savePostRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _replyToCommentRepository = replyToCommentRepository;
            _userManager = userManager;
            _friendRepository = friendRepository;
            _blockByUserRepository = blockByUserRepository;
            _savePostRepository = savePostRepository;
        }


        // Get Current User
        [HttpGet]
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
        [HttpGet]
        public IActionResult LikedPost()
        {
            var currentUserId = CurrentUser().Id;

            List<LikePost> FriendAndPublicPosts = new List<LikePost>();

            foreach (var item in  _likeRepository.GetAllLikedPost(currentUserId))
            {
                bool saved = _savePostRepository.CheckSave(item.Post.Id, currentUserId);
                if (saved)
                {
                    ViewBag.SavedPost = true;
                }
                else
                {
                    ViewBag.SavedPost = false;
                }


                bool blocked = _blockByUserRepository.CheckBlock(currentUserId, item.Post.UserId);
                // Check Blocked by Post publisher

                // Check If Friends
                bool friend = _friendRepository.CheckIfFriend(item.Post.UserId, currentUserId);


                if ((blocked == false && friend == true && item.Post.Audience == "Friends") ||
                    (blocked == false && friend == false && item.Post.Audience == "Public") ||
                     item.Post.UserId == currentUserId)
                {
                    FriendAndPublicPosts.Add(item);
                }
            }

            return View(FriendAndPublicPosts);
        }





        // Like Post
        [HttpPost]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = CurrentUser().Id;

            LikePost like = new LikePost()
            {
                UserId = userId,
                PostId = postId,
            };

            Post post = _postRepository.GetById(postId);
            post.LikeCount++;

            await _likeRepository.LikePost(like);

            return Json(new { success = true });
        }





        // UnLike Post
        [HttpPost]
        public async Task<IActionResult> UnlikePost(int postId)
        {

            var userId = CurrentUser().Id;

            Post post = _postRepository.GetById(postId);
            post.LikeCount--;

            await _likeRepository.UnLikePost(postId, userId);

            return Json(new { success = true }); // Return a JSON response
        }



        // Get All Post Likers
        [HttpGet]
        public IActionResult PostLikeUsers(int id)
        {
            var currentUserId = CurrentUser().Id;
            var likes =  _likeRepository.PostLikeUsers(id);
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
        [HttpPost]
        public async Task<IActionResult> LikeComment(int commentId)
        {
            var userId = CurrentUser().Id;

            LikeComment like = new LikeComment()
            {
                UserId = userId,
                CommentId = commentId,
            };

            Comment comment = _commentRepository.GetById(commentId);
            comment.LikeCount++;

            await _likeRepository.LikeComment(like);

            return Json(new { success = true });
        }




        // UnLike Comment
        [HttpPost]
        public async Task<IActionResult> UnLikeComment(int commentId)
        {
            var userId = CurrentUser().Id;

            Comment comment =  _commentRepository.GetById(commentId);
            comment.LikeCount--;

            await _likeRepository.UnLikeComment(commentId, userId);

            return Json(new { success = true }); // Return a JSON response
        }




        // Get All Comment Likers
        [HttpGet]
        public IActionResult CommentLikeUsers(int id)
        {
            var likes =  _likeRepository.CommentLikeUsers(id);
            return View(likes);
        }



        // Like Reply To Comment
        [HttpPost]
        public async Task<IActionResult> LikeReplyToComment(int replyId)
        {
            var userId = CurrentUser().Id;

            LikeReplyToComment like = new LikeReplyToComment()
            {
                UserId = userId,
                ReplyId = replyId
            };

            ReplyToComment reply =  _replyToCommentRepository.GetById(replyId);
            reply.LikeCount++;

            await _likeRepository.LikeReplyToComment(like);

            return Json(new { success = true });
        }




        // UnLike Reply To Comment
        [HttpPost]
        public async Task<IActionResult> UnLikeReplyToComment(int replyId)
        {
            var userId = CurrentUser().Id;

            ReplyToComment reply = _replyToCommentRepository.GetById(replyId);
            reply.LikeCount--;

            await _likeRepository.UnLikeReplyToComment(replyId, userId);

            return Json(new { success = true }); // Return a JSON response
        }


        // Get All Reply Likers
        [HttpGet]
        public IActionResult ReplyLikeUsers(int id)
        {
            var likes =  _likeRepository.ReplyLikeUsers(id);
            return View(likes);
        }


    }
}
