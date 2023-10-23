using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class ReplyToCommentController : Controller
    {
        private readonly IReplyToCommentRepository _replyToCommentRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlockByAdminRepository _blockByAdminRepository;

        public ReplyToCommentController(IReplyToCommentRepository replyToCommentRepository, ICommentRepository commentRepository,
            IPostRepository postRepository, UserManager<ApplicationUser> userManager, IBlockByAdminRepository blockByAdminRepository)
        {
            _replyToCommentRepository = replyToCommentRepository;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userManager = userManager;
            _blockByAdminRepository = blockByAdminRepository;
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




        // Add Reply To Comment
        [HttpPost]
        public async Task<IActionResult> AddReply(int commentId, string content, string userName)
        {
            try
            {
                string currentUserId = CurrentUser().Id;

                // Check if blocked by Admin
                bool blocked = _blockByAdminRepository.CheckBlock(currentUserId);
                if (blocked)
                {
                    ModelState.AddModelError(string.Empty, "Sorry, You can not publish because You are blocked by Admin");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        ModelState.AddModelError("", "Sorry, You can not publish nothing");
                    }
                    else
                    {
                        Comment comment = _commentRepository.GetById(commentId);
                        Post post = _postRepository.GetById(comment.PostId);


                        ReplyToComment reply = new ReplyToComment()
                        {
                            UserId = currentUserId,
                            CommentId = commentId,
                            Content = content,
                            ReplyUserName = userName,
                            PublishDate = DateTime.Now,
                        };

                        post.CommentCount++;
                        comment.CommentCount++;

                        await _replyToCommentRepository.Add(reply);
                        return RedirectToAction("ShowComments", "Post", new { postId = post.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }




        // Delete Reply To Comment
        [HttpPost]
        public async Task<IActionResult> DeleteReply(int replyId, int commentId)
        {
            try
            {
                Comment comment =  _commentRepository.GetById(commentId);
                Post post =  _postRepository.GetById(comment.PostId);
                comment.CommentCount--;
                post.CommentCount--;

                await _replyToCommentRepository.Delete(replyId);
                return RedirectToAction("ShowComments", "Post", new { postId = post.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false });
        }



    }
}
