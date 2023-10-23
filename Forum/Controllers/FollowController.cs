using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class FollowController : Controller
    {
        private readonly IFollowRepository _followRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowController(
            IFollowRepository followRepository,
            UserManager<ApplicationUser> userManager)
        {
            _followRepository = followRepository;
            _userManager = userManager;
        }



        // Get CurrentUser
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }



        // Get Followers
        [HttpGet]
        public IActionResult GetFollowers()
        {
            string currentUserId = CurrentUser().Id;
            var followers = _followRepository.GetFollowerByUserId(currentUserId);
            return View(followers);
        }




        // Get Followed By Me
        [HttpGet]
        public IActionResult GetFollowedsByMe()
        {
            string currentUserId = CurrentUser().Id;
            var followers = _followRepository.GetFollowedByUserId(currentUserId);
            return View(followers);
        }




        // Add Follow
        [HttpPost]
        public async Task<IActionResult> AddFollow(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest();
                }

                string currentUserId = CurrentUser().Id;
                Follow follow = new Follow()
                {
                    FollowedId = userId,
                    FollowerId = userId,
                    FollowDate = DateTime.Now
                };


                await _followRepository.Add(follow);
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        // Cancel Follow
        [HttpPost]
        public async Task<IActionResult> CancelFollow(string userId)
        {
            try
            {
                string currentUserId = CurrentUser().Id;
                Follow follow = _followRepository.GetByUserId(userId, currentUserId);
                if (follow == null)
                {
                    return NotFound();
                }
                await _followRepository.Delete(follow.Id);
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
