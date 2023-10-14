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

        public ReplyToCommentController(IReplyToCommentRepository replyToCommentRepository, ICommentRepository commentRepository, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _replyToCommentRepository = replyToCommentRepository;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userManager = userManager;
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




        // Add Reply To Comment
        public async Task<ActionResult> AddReply(int commentId, string content, string userName)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(content))
                {
                    ModelState.AddModelError("", "Sorry, You can not publish nothing");
                }
                else
                {
                    Comment comment = await _commentRepository.GetById(commentId);
                    Post post = await _postRepository.GetById(comment.PostId);

                    string userId = CurrentUser().Id;

                    ReplyToComment reply = new ReplyToComment()
                    {
                        UserId = userId,
                        CommentId = commentId,
                        Content = content,
                        ReplyUserName = userName,
                        PublishDate = DateTime.Now,
                    };

                    post.CommentCount++;
                    comment.CommentCount++;

                    await _replyToCommentRepository.Add(reply);
                    return RedirectToAction("ShowComments", "Post", new {postId = post.Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }




        // Delete Reply To Comment
        public async Task<IActionResult> DeleteReply(int replyId, int commentId)
        {
            try
            {
                Comment comment = await _commentRepository.GetById(commentId);
                Post post = await _postRepository.GetById(comment.PostId);
                comment.CommentCount--;
                post.CommentCount--;

                 await _replyToCommentRepository.Delete(replyId);
                return RedirectToAction("ShowComments", "Post", new {postId = post.Id});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false });
        }



    }
}
