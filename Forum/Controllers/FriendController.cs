using Forum.IRepository;
using Forum.IRepository.Repository;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class FriendController : Controller
    {
        private readonly IFriendRepository _friendRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlockByUserRepository _blockByUserRepository;
        private readonly IFollowRepository _followRepository;

        public FriendController(
            IFriendRepository friendRepository,
            UserManager<ApplicationUser> userManager,
            IBlockByUserRepository blockByUserRepository,
            IFollowRepository followRepository)
        {
            _friendRepository = friendRepository;
            _userManager = userManager;
            _blockByUserRepository = blockByUserRepository;
            _followRepository = followRepository;
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


        // check block
        [HttpGet]
        public bool BlockedByUser(string userId)
        {
            string currentUserId = CurrentUser().Id;
            bool blockedToMe = _blockByUserRepository.CheckBlock(currentUserId, userId);
            bool blockedFromMe = _blockByUserRepository.CheckBlock(userId, currentUserId);
            if (blockedFromMe || blockedToMe)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // Add Request
        [HttpPost]
        public async Task<IActionResult> AddRequest(string userId)
        {
            bool blocked = BlockedByUser(userId);
            if (!blocked)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest();
                }

                ApplicationUser currentUser = CurrentUser();
                FriendRequest request = new FriendRequest()
                {
                    SenderId = currentUser.Id,
                    RecieverId = userId,

                };

                bool followed = _followRepository.CheckFollow(userId, currentUser.Id);
                if (!followed)
                {
                    Follow follow = new Follow()
                    {
                        FollowedId = userId,
                        FollowerId = currentUser.Id,
                        FollowDate = DateTime.Now
                    };

                    await _followRepository.Add(follow);
                }


                await _friendRepository.AddRequest(request);
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }



        // Cancel Request
        [HttpPost]
        public async Task<IActionResult> CancelRequest(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            ApplicationUser currentUser = CurrentUser();

            await _friendRepository.CancelRequest(userId, currentUser.Id);

            // Cancel follow
            Follow followed = _followRepository.GetByUserId(userId,currentUser.Id);

            await _followRepository.Delete(followed.Id);


            return Json(new { success = true });
        }





        // Accept and Reject Request on profile
        [HttpPost]
        public async Task<IActionResult> AcceptRequest(string userId, bool accept)
        {
            try
            {

                string currentUserId = CurrentUser().Id;
                FriendRequest request = _friendRepository.GetRequestByUsersId( userId, currentUserId);
                // If Accept
                if (accept == true)
                {
                    Friend friend = new Friend()
                    {
                        UserOneId = request.SenderId,
                        UserTwoId = request.RecieverId,
                    };
                    // Add friend and delete request
                    await _friendRepository.AcceptRequest(friend);
                    await _friendRepository.RejectRequest(request);

                    Follow follow = new Follow()
                    {
                        FollowedId = userId,
                        FollowerId = currentUserId,
                        FollowDate = DateTime.Now
                    };

                    await _followRepository.Add(follow);

                }
                else
                {
                    // if Reject 
                    // Delete request
                    await _friendRepository.RejectRequest(request);
                    // Cancel follow
                    Follow followed = _followRepository.GetByUserId(currentUserId,userId);

                    await _followRepository.Delete(followed.Id);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // All Request to me
        [HttpGet]
        public IActionResult MyRequest()
        {
            ApplicationUser currentUser = CurrentUser();
            var requests = _friendRepository.FriendRequests(currentUser.Id);
            return View(requests);
        }




        // All Request to other
        [HttpGet]
        public IActionResult RequestFromMe()
        {
            ApplicationUser currentUser = CurrentUser();
            var requests = _friendRepository.MyFriendRequests(currentUser.Id);
            return View(requests);
        }




        // UnFriend someone
        [HttpGet]
        public async Task<IActionResult> DeleteFriend(int id)
        {
            var friend = _friendRepository.GetFriendById(id);
            if (friend == null)
            {
                return BadRequest();
            }

            await _friendRepository.DeleteFriend(friend);
            return RedirectToAction("MyFriend");
        }


        // UnFriend from someone profile 
        [HttpGet]
        public async Task<IActionResult> RemoveFriend(string userId)
        {
            string currentUserId = CurrentUser().Id;
            var friend = _friendRepository.GetFriendByUsersId(userId, currentUserId);
            if (friend == null)
            {
                return BadRequest();
            }

            await _friendRepository.DeleteFriend(friend);

            // Cancel follow
            Follow follower = _followRepository.GetByUserId(userId, currentUserId);

            await _followRepository.Delete(follower.Id);

            // Cancel follow
            Follow followed = _followRepository.GetByUserId(currentUserId, userId);

            await _followRepository.Delete(followed.Id);

            return RedirectToAction("ProfileSomeOne", "Post", new { userId });
        }



        // All My friends
        [HttpGet]
        public IActionResult MyFriend()
        {
            ApplicationUser currentUser = CurrentUser();
            var friends = _friendRepository.MyFriends(currentUser.Id);
            return View(friends);
        }

    }
}

