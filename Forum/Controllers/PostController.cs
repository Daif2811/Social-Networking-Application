using Forum.Custom_Attributes;
using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Win32;
using NuGet.Versioning;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;
        private readonly IFriendRepository _friendRepository;
        private readonly IBlockByAdminRepository _blockByAdminRepository;
        private readonly IBlockByUserRepository _blockByUserRepository;

        public PostController(IPostRepository postRepository, IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager, IFriendRepository friendRepository,
            IBlockByAdminRepository blockByAdminRepository, IBlockByUserRepository blockByUserRepository)
        {
            _postRepository = postRepository;
            _environment = environment;
            _userManager = userManager;
            _friendRepository = friendRepository;
            _blockByAdminRepository = blockByAdminRepository;
            _blockByUserRepository = blockByUserRepository;
        }

        // Get Current User
        public ApplicationUser CurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }


        // My Profile
        public IActionResult MyProfile()
        {
            ApplicationUser user = CurrentUser();
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.User = user;
            var posts = _postRepository.Profile(user.Id);

            return View(posts);
        }


        // Profile Someone
        public async Task<IActionResult> ProfileSomeOne(string userId)
        {
            ApplicationUser currentUser = CurrentUser();

            // check if Admin did block
            bool blockedByAdmin = _blockByAdminRepository.CheckBlock(userId);
            ViewBag.BlockedByAdmin = blockedByAdmin;


            // check if User did block
            bool blockedByUser = _blockByUserRepository.CheckBlock(userId, currentUser.Id);
            ViewBag.BlockedByUser = blockedByUser;



            if (userId == currentUser.Id)
            {
                return RedirectToAction("MyProfile");
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.User = user;


            var friend = _friendRepository.CheckFriend(userId, currentUser.Id);
            if (friend != null)
            {
                // He is friend
                ViewBag.Found = "Friend";
            }
            else
            {
                var found = _friendRepository.CheckRequest(userId, currentUser.Id);
                // I sent friend request
                if (found != null)
                {

                    ViewBag.Found = "MyRequest";
                }
                else
                {
                    var me = _friendRepository.CheckRequest(currentUser.Id, userId);
                    // He sent to me friend request
                    if (me != null)
                    {
                        ViewBag.Found = "HisRequest";

                    }
                    else
                    {
                        // No request
                        ViewBag.Found = "NoRequest";

                    }
                }
            }

            var posts = _postRepository.Profile(userId);

            return View(posts);
        }





        // GET All Posts
        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser();
            List<Post> posts = new List<Post>();

            string currentUserId = CurrentUser().Id;
            foreach (var item in _postRepository.GetAll())
            {
                bool blocked = _blockByUserRepository.CheckBlock(currentUserId,item.UserId );
                if (!blocked)
                {
                    posts.Add(item);
                }

            }

            return View(posts);
        }



        // Details and Show All Comments
        public IActionResult ShowComments(int postId)
        {
            var post = _postRepository.GetAllComments(postId);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }



        // Create New Post
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CheckIfBlockedByAdmin]
        public async Task<IActionResult> Create(Post post, IFormFile image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get CurrentUserId
                    string currentUserId = CurrentUser().Id;

                    // Check if Blocked by Admin
                    bool blockedByAdmin = _blockByAdminRepository.CheckBlock(currentUserId);
                    if (blockedByAdmin)
                    {
                        ModelState.AddModelError(string.Empty, "Sorry, You can not publish because You are blocked by Admin");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(post.Content) && image == null)
                        {
                            ModelState.AddModelError(string.Empty, "Sorry, You can not publish an empty post");
                        }
                        else
                        {
                            if (image != null)
                            {
                                // Save the image file to the wwwroot/images folder
                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(_environment.WebRootPath, "Post-Images", fileName);
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                post.Image = fileName;
                            }

                            post.UserId = currentUserId;
                            post.PublishDate = DateTime.Now;

                            _postRepository.Add(post, image);
                            return RedirectToAction("Index");
                        }
                    }
                }
                return View(post);
            }
            catch (Exception ex)
            {
                return View("", ex.Message);
            }
        }




        // Edit Post
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = CurrentUser();
            if (id == null)
            {
                return BadRequest();
            }
            var post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.UserId != currentUser.Id)
            {
                ModelState.AddModelError("", "Sorry You can't Edit this post , Because it is not yours");
            }
            TempData["oldImage"] = post.Image;
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldImage = TempData["oldImage"] as string;


                    if (string.IsNullOrWhiteSpace(post.Content) && image == null && oldImage == null)
                    {
                        ModelState.AddModelError(string.Empty, "Sorry, You can not publish an empty post");
                    }
                    if (image == null)
                    {
                        if (oldImage != null)
                        {
                            post.Image = oldImage;
                        }
                    }

                    if (image != null)
                    {
                        if (oldImage != null)
                        {
                            var oldPath = Path.Combine(_environment.WebRootPath, "Post-Images", oldImage);
                            System.IO.File.Delete(oldPath);
                        }
                        // Save the image file to the wwwroot/images folder
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "Post-Images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        post.Image = fileName;
                    }

                    //Claim userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    //string userId = userIdClaim.Value;   // Get the value
                    string userId = CurrentUser().Id;

                    post.UserId = userId;
                    post.PublishDate = DateTime.Now;


                    await _postRepository.Update(post, image);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(post);
        }







        // Delete Post
        public async Task<IActionResult> Delete(int id)
        {
            string userId = CurrentUser().Id;

            if (id == null)
            {
                return BadRequest();
            }
            var post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }


            if (post.UserId != userId)
            {
                ModelState.AddModelError("", "Sorry, This post is not yours");
            }

            return View(post);

        }

        [HttpPost]
        [ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;

                var post = await _postRepository.GetById(id);
                if (post == null)
                {
                    return NotFound();
                }
                if (post.UserId != userId)
                {
                    ModelState.AddModelError("", "Sorry, This post is not yours");
                }

                await _postRepository.Delete(id);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }


        }





    }
}
