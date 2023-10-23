using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class PostReportController : Controller
    {
        private readonly IPostReportRepository _postReportRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;

        public PostReportController(
            IPostReportRepository postReportRepository,
            UserManager<ApplicationUser> userManager,
            IPostRepository postRepository)
        {
            _postReportRepository = postReportRepository;
            _userManager = userManager;
            _postRepository = postRepository;
        }



        // Get CurrentUser
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            Claim UserClaim = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            string userId = UserClaim.Value;
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }



        // Get All Post Reports
        [HttpGet]
        public IActionResult AllPostReports()
        {
            var reports = _postReportRepository.GetAllReports();
            return View(reports);
        }




        // Create Report
        [HttpGet]
        public IActionResult AddReport(int postId)
        {
            Post post = _postRepository.GetById(postId);
            if (post == null)
            {
                return BadRequest();
            }

            TempData["postId"] = postId;

            return View();
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReport(string SelectedType)
        {
            try
            {
                PostReport report = new PostReport();
                int postId = (int)TempData["postId"];
                ApplicationUser currentUser = CurrentUser();
                report.ReporterId = currentUser.Id;
                report.PostId = postId;
                report.Type = SelectedType;

               await _postReportRepository.Add(report);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return Json (new { success = true });
        }


      

      
    }
}
