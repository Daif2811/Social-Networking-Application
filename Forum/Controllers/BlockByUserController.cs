﻿using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class BlockByUserController : Controller
    {

        private readonly IBlockByUserRepository _blockByUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFriendRepository _friendRepository;

        public BlockByUserController(
            IBlockByUserRepository blockByUserRepository,
            UserManager<ApplicationUser> userManager,
            IFriendRepository friendRepository)
        {
            _blockByUserRepository = blockByUserRepository;
            _userManager = userManager;
            _friendRepository = friendRepository;
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
                var friend = _friendRepository.CheckFriend(userId, currentUserId);
                if (friend != null)
                {
                    await _friendRepository.DeleteFriend(friend);
                }
                else
                {
                    var requsetFromMe = _friendRepository.CheckRequest(userId, currentUserId);
                    if (requsetFromMe != null)
                    {
                        //_friendRepository.CancelRequest(requset.RecieverId, requset.RecieverId);
                        await _friendRepository.CancelRequest(userId, currentUserId);
                    }
                    else
                    {
                        var requsetToMe = _friendRepository.CheckRequest(currentUserId, userId);
                        if (requsetToMe != null)
                        {
                            await _friendRepository.CancelRequest(currentUserId, userId);
                        }
                    }
                }

                return Json(new { success = true });

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest (ex.Message);
            }
        }


    }
}