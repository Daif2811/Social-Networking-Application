using Forum.IRepository;
using Forum.Models;
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

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Search(string searchName)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                var searchResult = _postRepository.GetAll().Where(a => a.User.UserName.Contains(searchName)
                || a.Content.Contains(searchName)).ToList();
                if (searchResult != null)
                {
                    return View(searchResult);
                }

                return RedirectToAction("Index", "Post");
            }
            return RedirectToAction("Index", "Post");
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}