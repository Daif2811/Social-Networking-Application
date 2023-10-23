using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            IPostRepository postRepository,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.url = TempData["url"];
            return View();
        }


        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }



        // Get Current URL
        [HttpGet]
        public IActionResult CaptureCurrentURL()
        {
            // Get the current URL
            var currentUrl = HttpContext.Request.GetDisplayUrl();

            // You can also access different parts of the URL, e.g., currentUrl.Scheme, currentUrl.Host, currentUrl.Path, currentUrl.QueryString, etc.

            // You can use currentUrl as needed
            // ...
            TempData["url"] = ViewBag.CurrentURL = currentUrl;

            return RedirectToAction("Index");
        }




        //// Search
        //[HttpGet]
        //public IActionResult Search(string searchName)
        //{
        //    if (!string.IsNullOrEmpty(searchName))
        //    {
        //        var searchResult = _postRepository.Search(searchName);
        //        return View(searchResult);

        //    }
        //    return RedirectToAction("Index", "Post");

        //}



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}




      

    }
}