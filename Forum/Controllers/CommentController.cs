using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlockByAdminRepository _blockByAdminRepository;

        public CommentController(
            ICommentRepository commentRepository,
            IPostRepository postRepository,
            UserManager<ApplicationUser> userManager,
            IBlockByAdminRepository blockByAdminRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userManager = userManager;
            _blockByAdminRepository = blockByAdminRepository;
        }


        // Get CurrentUser
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            //Claim userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            //string userId = userIdClaim.Value;   // Get the value

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }



        // Add Comment
        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string commentText)
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
                    if (string.IsNullOrWhiteSpace(commentText))
                    {
                        ModelState.AddModelError(string.Empty, "Sorry, You can not publish an empty comment");
                    }
                    else
                    {
                        Comment comment = new Comment()
                        {
                            UserId = currentUserId,
                            Content = commentText,
                            PostId = postId,
                            PublishDate = DateTime.Now
                        };

                        Post post = _postRepository.GetById(postId);
                        post.CommentCount++;

                        await _commentRepository.Add(comment);
                        return RedirectToAction("ShowComments", "Post", new { postId });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("ShowComments", "Post", new { postId });
        }



        // Edit Comment
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(comment.Content))
                {
                    ModelState.AddModelError("", "You can not comment nothing");
                }
                else
                {
                    string userId = CurrentUser().Id;

                    comment.UserId = userId;
                    comment.PublishDate = DateTime.Now;

                    await _commentRepository.Update(comment);
                    return RedirectToAction("ShowComments", "Post", new { comment.PostId });
                }
            }
            return View(comment);
        }



        // Delete Comment
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }




        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Get post id to delete count of comment and reply from post comment count
            Post post = _postRepository.GetById(comment.PostId);

            post.CommentCount -= (comment.ReplyToComments.Count + 1);

            await _commentRepository.Delete(id);
            return RedirectToAction("ShowComments", "Post", new { comment.PostId });
        }



    }
}
