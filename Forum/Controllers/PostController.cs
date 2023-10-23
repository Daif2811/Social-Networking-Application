using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFriendRepository _friendRepository;
        private readonly IBlockByAdminRepository _blockByAdminRepository;
        private readonly IBlockByUserRepository _blockByUserRepository;
        private readonly IFollowRepository _followRepository;
        private readonly ISavePostRepository _savePostRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(
            IPostRepository postRepository,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            IFriendRepository friendRepository,
            IBlockByAdminRepository blockByAdminRepository,
            IBlockByUserRepository blockByUserRepository,
            IFollowRepository followRepository,
            ISavePostRepository savePostRepository,
            ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _environment = environment;
            _userManager = userManager;
            _friendRepository = friendRepository;
            _blockByAdminRepository = blockByAdminRepository;
            _blockByUserRepository = blockByUserRepository;
            _followRepository = followRepository;
            _savePostRepository = savePostRepository;
            _logger = logger;
        }

        // Get Current User
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }



        // My Profile
        [HttpGet]
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
        [HttpGet]
        public IActionResult ProfileSomeOne(string userId)
        {
            ApplicationUser currentUser = CurrentUser();

            // check if Admin did block
            bool blockedByAdmin = _blockByAdminRepository.CheckBlock(userId);
            ViewBag.BlockedByAdmin = blockedByAdmin;


            // check if User did block
            bool blockedByUser = _blockByUserRepository.CheckBlock(userId, currentUser.Id);
            ViewBag.BlockedByUser = blockedByUser;


            bool ckeckfollow = _followRepository.CheckFollow(userId, currentUser.Id);
            ViewBag.Followed = ckeckfollow;


            if (userId == currentUser.Id)
            {
                return RedirectToAction("MyProfile");
            }

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
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

            List<Post> FriendAndPublicPosts = new List<Post>();
            foreach (var item in _postRepository.Profile(userId))
            {
                bool saved = _savePostRepository.CheckSave(item.Id, currentUser.Id);
                if (saved)
                {
                    ViewBag.SavedPost = true;
                }
                else
                {
                    ViewBag.SavedPost = false;
                }

                // Check If Friends
                bool checkFriend = _friendRepository.CheckIfFriend(item.UserId, currentUser.Id);


                if ((blockedByUser == false && checkFriend == true && item.Audience == "Friends") ||
                    (blockedByUser == false && checkFriend == false && item.Audience == "Public") ||
                     item.UserId == currentUser.Id)
                {
                    FriendAndPublicPosts.Add(item);
                }
            }

            return View(FriendAndPublicPosts);





        }


        // GET All Posts
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser();



            List<Post> FriendAndPublicPosts = new List<Post>();

            string currentUserId = CurrentUser().Id;
            foreach (var item in _postRepository.GetAll())
            {
                // Check Post Saved 
                bool saved = _savePostRepository.CheckSave(item.Id, currentUserId);
                if (saved)
                {
                    ViewBag.SavedPost = true;
                }
                else
                {
                    ViewBag.SavedPost = false;
                }


                // Check Blocked by Post publisher
                bool blocked = _blockByUserRepository.CheckBlock(currentUserId, item.UserId);

                // Check If Friends
                bool friend = _friendRepository.CheckIfFriend(item.UserId, currentUserId);


                if ((blocked == false && friend == true && item.Audience == "Friends") ||
                    (blocked == false && friend == false && item.Audience == "Public") ||
                    item.UserId == currentUserId)
                {
                    FriendAndPublicPosts.Add(item);
                }
            }

            return View(FriendAndPublicPosts);
        }



        // Details and Show All Comments
        [HttpGet]
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
        [HttpGet]
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

                            await _postRepository.Add(post);
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var currentUser = CurrentUser();

                var post = _postRepository.GetById(id);
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
            catch (Exception ex)
            {
                return View(ex.Message);

            }
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


                    await _postRepository.Update(post);
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
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                string userId = CurrentUser().Id;

                var post = _postRepository.GetById(id);
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
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                var userId = CurrentUser().Id;

                var post = _postRepository.GetById(id);
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
                // return Json(new {success = true});
            }
            catch (Exception ex)
            {
                //  ModelState.AddModelError("", ex.Message);
                return View(ex.Message);
            }


        }





    }
}
