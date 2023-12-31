﻿using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class UserReportController : Controller
    {
        private readonly IUserReportRepository _userReportRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserReportController(
            IUserReportRepository userReportRepository,
            UserManager<ApplicationUser> userManager)
        {
            _userReportRepository = userReportRepository;
            _userManager = userManager;
        }

        // Get CurrentUser
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUer = _userManager.FindByIdAsync(userId).Result;
            return currentUer;
        }



        // Get All User reports
        [HttpGet, Authorize(Roles ="Admin")]
        public IActionResult AllUserReports()
        {
            var reports = _userReportRepository.GetAllReports();

            return View(reports);
        }




        // Add Report
        [HttpGet]
        public IActionResult AddReport(string userId)
        {
            ViewBag.UserId = userId;
            TempData["userId"] = ViewBag.UserId;
            return View();
        }


        [HttpPost, ActionName("AddReport")]
        
        public async Task<IActionResult> ConfirmAddReport(string selectedType)
        {
            try
            {
                ApplicationUser currentUser = CurrentUser();
                UserReport report = new UserReport();
                string userId = TempData["userId"].ToString();
                report.Type = selectedType;
                report.ReporterId = currentUser.Id;
                report.UserId = userId;
                report.ReportDate = DateTime.Now;
                await _userReportRepository.Add(report);
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

        }

    }

}
