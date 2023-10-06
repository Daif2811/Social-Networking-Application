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

        public FriendController(IFriendRepository friendRepository, UserManager<ApplicationUser> userManager)
        {
            _friendRepository = friendRepository;
            _userManager = userManager;
        }







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



        public IActionResult AddRequest(string userId)
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

            _friendRepository.Request(request);
            return Json(new { success = true });
        }

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


        public IActionResult AcceptRequest(int id, bool accept)
        {
            FriendRequest request = _friendRepository.GetRequestById(id);
            if (accept == true)
            {

                Friend friend = new Friend()
                {
                    UserOneId = request.SenderId,
                    UserTwoId = request.RecieverId,
                };
                _friendRepository.AcceptRequest(friend);
                _friendRepository.RejectRequest(request);

            }
            else
            {
                _friendRepository.RejectRequest(request);
            }
            return Json(new {success = true});
        }


        public IActionResult MyFriend()
        {
            ApplicationUser currentUser = CurrentUser();
            var friends = _friendRepository.MyFriends(currentUser.Id);
            return View(friends);
        }


        public IActionResult MyRequest()
        {
            ApplicationUser currentUser = CurrentUser();
            var requests = _friendRepository.FriendRequests(currentUser.Id);
            return View(requests);
        }

        public IActionResult RequestFromMe()
        {
            ApplicationUser currentUser = CurrentUser();
            var requests = _friendRepository.MyFriendRequests(currentUser.Id);
            return View(requests);
        }


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

        

    }
}
