using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class BlockByAdminController : Controller
    {
        private readonly IBlockByAdminRepository _blockByAdminRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlockByAdminController(IBlockByAdminRepository blockByAdminRepository, UserManager<ApplicationUser> userManager)
        {
            _blockByAdminRepository = blockByAdminRepository;
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
            var blocks = _blockByAdminRepository.GetAll();
            return View(blocks);
        }

        // GET: BlockByAdminController/Details/5
        public ActionResult Details(int id)
        {
            BlockByAdmin block = _blockByAdminRepository.GetById(id);
            return View(block);
        }


        // POST: BlockByAdminController/Create
        //[HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult CreateBlock(string userId)
        {
            try
            {
                bool blocked = _blockByAdminRepository.CheckBlock(userId);

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest();
                }

                string adminId = CurrentUser().Id;

                BlockByAdmin Block = new BlockByAdmin()
                {
                    AdminId = adminId,
                    UserId = userId,
                    BlockDate = DateTime.Now,
                };

                _blockByAdminRepository.Add(Block);
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
                BlockByAdmin block = _blockByAdminRepository.GetByUserId(userId);
                if (block != null)
                {
                    _blockByAdminRepository.Delete(block.Id);
                    return Json(new {success = true});
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex) 
            {
                return View(ex.Message);
            }
        }
    }
}
