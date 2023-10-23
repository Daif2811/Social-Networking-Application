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

        public FriendController(IFriendRepository friendRepository, UserManager<ApplicationUser> userManager,
            IBlockByUserRepository blockByUserRepository)
        {
            _friendRepository = friendRepository;
            _userManager = userManager;
            _blockByUserRepository = blockByUserRepository;
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

        public bool BlockedByUser(string userId)
        {
            bool blocked = _blockByUserRepository.CheckBlock(CurrentUser().Id, userId);
            if (blocked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // Add Request
        public IActionResult AddRequest(string userId)
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

                _friendRepository.AddRequest(request);
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }


        // Cancel Request
        public IActionResult CancelRequest(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            ApplicationUser currentUser = CurrentUser();

            _friendRepository.CancelRequest(userId, currentUser.Id);
            return Json(new { success = true });
        }



        // Accept and Reject Request
        public IActionResult AcceptRequest(int id, bool accept)
        {
            FriendRequest request = _friendRepository.GetRequestById(id);
            // If Accept
            if (accept == true)
            {
                Friend friend = new Friend()
                {
                    UserOneId = request.SenderId,
                    UserTwoId = request.RecieverId,
                };
                // Add friend and delete request
                _friendRepository.AcceptRequest(friend);
                _friendRepository.RejectRequest(request);

            }
            else
            {
                // if Reject 
                // Delete request
                _friendRepository.RejectRequest(request);
            }
            return Json(new { success = true });
        }


        public IActionResult AcceptRequestOnProfile(string userId, bool accept)
        {
            string currentUserId = CurrentUser().Id;
            FriendRequest request = _friendRepository.CheckRequest(currentUserId, userId);
            // If Accept
            if (accept == true)
            {
                Friend friend = new Friend()
                {
                    UserOneId = request.SenderId,
                    UserTwoId = request.RecieverId,
                };
                // Add friend and delete request
                _friendRepository.AcceptRequest(friend);
                _friendRepository.RejectRequest(request);

            }
            else
            {
                // if Reject 
                // Delete request
                _friendRepository.RejectRequest(request);
            }
            return Json(new { success = true });
        }





        // All Request to me
        public IActionResult MyRequest()
        {
            ApplicationUser currentUser = CurrentUser();
            var requests = _friendRepository.FriendRequests(currentUser.Id);
            return View(requests);
        }




        // All Request to other
        public IActionResult RequestFromMe()
        {
            ApplicationUser currentUser = CurrentUser();
            var requests = _friendRepository.MyFriendRequests(currentUser.Id);
            return View(requests);
        }



        // All My friends
        public IActionResult MyFriend()
        {
            ApplicationUser currentUser = CurrentUser();
            var friends = _friendRepository.MyFriends(currentUser.Id);
            return View(friends);
        }


        // UnFriend someone
        public IActionResult DeleteFriend(int id)
        {
            var friend = _friendRepository.GetFriendById(id);
            if (friend == null)
            {
                return BadRequest();
            }

            _friendRepository.DeleteFriend(friend);
            return RedirectToAction("MyFriend");
        }
        public IActionResult RemoveFriend(string userId)
        {
            string currentUserId = CurrentUser().Id;
            var friend = _friendRepository.CheckFriend(userId, currentUserId);
            if (friend == null)
            {
                return BadRequest();
            }

            _friendRepository.DeleteFriend(friend);
            return RedirectToAction("ProfileSomeOne", "Post", new { userId });
        }



    }
}
