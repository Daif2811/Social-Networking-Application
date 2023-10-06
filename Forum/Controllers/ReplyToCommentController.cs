using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class ReplyToCommentController : Controller
    {
        private readonly IReplyToCommentRepository _replyToCommentRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public ReplyToCommentController(IReplyToCommentRepository replyToCommentRepository, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _replyToCommentRepository = replyToCommentRepository;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        

        public async Task<ActionResult> AddReply(int commentId, string content)
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

                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                   // ViewBag.UserName = userName;
                    ReplyToComment reply = new ReplyToComment()
                    {
                        UserId = userId,
                        CommentId = commentId,
                        Content = content,
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
