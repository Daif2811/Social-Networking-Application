using Forum.IRepository;
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

        public BlockByUserController(IBlockByUserRepository blockByUserRepository, UserManager<ApplicationUser> userManager)
        {
            _blockByUserRepository = blockByUserRepository;
            _userManager = userManager;
        }


        public ApplicationUser CurrentUser()
        {
            //Claim userClaim = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            //string userId = userClaim.Value;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;


        }



        // GET: BlockByAdminController
        public ActionResult AllBlocks()
        {
            var blocks = _blockByUserRepository.GetAll();
            return View(blocks);
        }

        // GET: BlockByAdminController/Details/5
        public ActionResult Details(int id)
        {
            BlockByUser block = _blockByUserRepository.GetById(id);
            return View(block);
        }


        // POST: BlockByAdminController/Create
        public ActionResult CreateBlock(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest();
                }

                string blockerId = CurrentUser().Id;

                BlockByUser Block = new BlockByUser()
                {
                    BlockerId = blockerId,
                    UserId = userId,
                    BlockDate = DateTime.Now,
                };

                _blockByUserRepository.Add(Block);
                return Json(new { success = true });

            }

            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }



        // POST: BlockByAdminController/Delete/5
        public ActionResult CancelBlock(string userId)
        {
            try
            {

                BlockByUser block = _blockByUserRepository.GetByUserId(userId);
                if (block != null)
                {
                    _blockByUserRepository.Delete(block.Id);
                    return Json(new { success = true });
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }








    }
}
