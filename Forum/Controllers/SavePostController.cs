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
    public class SavePostController : Controller
    {
        private readonly ISavePostRepository _savePostRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IBlockByUserRepository _blockByUserRepository;
        private readonly IFriendRepository _friendRepository;

        public SavePostController(
            ISavePostRepository savePostRepository,
            UserManager<ApplicationUser> userManager,
            IPostRepository postRepository,
            IBlockByUserRepository blockByUserRepository,
            IFriendRepository friendRepository)
        {
            _savePostRepository = savePostRepository;
            _userManager = userManager;
            _postRepository = postRepository;
            _blockByUserRepository = blockByUserRepository;
            _friendRepository = friendRepository;
        }



        // Get Current User
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(currentUserId).Result;
            return currentUser;
        }



        // Get Saved Posts
        [HttpGet]
        public IActionResult SavedPost()
        {
            List<SavePost> FriendAndPublicPosts = new List<SavePost>();

            string currentUserId = CurrentUser().Id;
            foreach (var item in _savePostRepository.GetAllByUserId(currentUserId))
            {
                // Check Blocked by Post publisher
                bool blocked = _blockByUserRepository.CheckBlock(currentUserId, item.Post.UserId);


                // Check If Friends
                bool friend = _friendRepository.CheckFriend(item.Post.UserId, currentUserId);


                bool saved = _savePostRepository.CheckSave(item.PostId, currentUserId);
                if (saved)
                {
                    ViewBag.SavedPost = true;
                }
                else
                {
                    ViewBag.SavedPost = false;
                }


                if ((blocked == false && friend == true && item.Post.Audience == "Friends") ||
                    (blocked == false && friend == false && item.Post.Audience == "Public") ||
                    item.Post.UserId == currentUserId)
                {
                    FriendAndPublicPosts.Add(item);
                }
            }

            return View(FriendAndPublicPosts);


        }



        // Save Post
        [HttpPost]
        public async Task<IActionResult> SavePost(int postId)
        {
            try
            {
                string currentUserId = CurrentUser().Id;
                bool CheckSave = _savePostRepository.CheckSave(postId, currentUserId);
                if (!CheckSave)
                {
                    SavePost post = new SavePost()
                    {
                        PostId = postId,
                        UserId = currentUserId
                    };

                    await _savePostRepository.Add(post);

                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }



        // Cancel Save

        public async Task<IActionResult> CancelSave(int postId)
        {
            try
            {
                string currentUserId = CurrentUser().Id;
                SavePost post = _savePostRepository.GetByPostIdAndUserId(postId, currentUserId);
                if (post != null)
                {
                    await _savePostRepository.Delete(post.Id);
                    return Json(new { success = true });

                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }



    }
}
