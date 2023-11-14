using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class BlockByUserController : Controller
    {

        private readonly IBlockByUserRepository _blockByUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFriendRepository _friendRepository;
        private readonly IFollowRepository _followRepository;

        public BlockByUserController(
            IBlockByUserRepository blockByUserRepository,
            UserManager<ApplicationUser> userManager,
            IFriendRepository friendRepository,
            IFollowRepository followRepository)
        {
            _blockByUserRepository = blockByUserRepository;
            _userManager = userManager;
            _friendRepository = friendRepository;
            _followRepository = followRepository;
        }



        // Get Current User
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            //Claim userClaim = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            //string userId = userClaim.Value;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;


        }



        // Get All Blocked User for CurrentUser
        [HttpGet]
        public ActionResult AllBlocks()
        {
            string currentUserId = CurrentUser().Id;
            var blocks = _blockByUserRepository.GetAllByUser(currentUserId);
            return View(blocks);
        }




        // POST: Create Block
        [HttpPost]
        public async Task<ActionResult> CreateBlock(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest();
                }

                string currentUserId = CurrentUser().Id;

                BlockByUser Block = new BlockByUser()
                {
                    BlockerId = currentUserId,
                    UserId = userId,
                    BlockDate = DateTime.Now,
                };

                await _blockByUserRepository.Add(Block);

                var followedByMe = _followRepository.GetByUserId(userId, currentUserId);
                var followedToMe = _followRepository.GetByUserId(currentUserId, userId);
                var friend = _friendRepository.CheckFriend(userId, currentUserId);
                var requsetFromMe = _friendRepository.CheckRequest(currentUserId,userId);
                var requsetToMe = _friendRepository.CheckRequest(userId, currentUserId);
                 
                if (followedByMe != null)
                {
                    await _followRepository.Delete(followedByMe.Id);
                }
                if (followedToMe != null)
                {
                    await _followRepository.Delete(followedToMe.Id);
                }
                if (friend)
                {
                    Friend friend1 = _friendRepository.GetFriendByUsersId(userId, currentUserId);
                    await _friendRepository.DeleteFriend(friend1);
                }
                if (requsetFromMe)
                {
                    await _friendRepository.CancelRequest(userId, currentUserId);
                }
                if (requsetToMe)
                {
                    await _friendRepository.CancelRequest(currentUserId, userId);
                }



                return Json(new { success = true });

            }

            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



        // POST: Cancel Block
        [HttpPost]
        public async Task<ActionResult> CancelBlock(string userId)
        {
            try
            {
                BlockByUser block = _blockByUserRepository.GetByUserId(userId);
                if (block != null)
                {
                    await _blockByUserRepository.Delete(block.Id);
                    return Json(new { success = true });
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
