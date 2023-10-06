﻿using Forum.IRepository;
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

        public PostController(IPostRepository postRepository, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, IFriendRepository friendRepository)
        {
            _postRepository = postRepository;
            _environment = environment;
            _userManager = userManager;
            _friendRepository = friendRepository;
        }


        public ApplicationUser CurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }


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

        public async Task<IActionResult> ProfileSomeOne(string userId)
        {
            ApplicationUser currentUser = CurrentUser();
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
                var found = _friendRepository.GetRequestByUserId(userId, currentUser.Id);
                // I sent friend request
                if (found != null)
                {

                    ViewBag.Found = "MyRequest";
                }
                else
                {
                    var me = _friendRepository.GetRequestByUserId(currentUser.Id, userId);
                    // He sent to me friend request
                    if (me != null)
                    {
                        ViewBag.Found = "HisRequest";

                    }
                    else
                    {
                        ViewBag.Found = "NoRequest";

                    }
                }
            }



            var posts = _postRepository.Profile(userId);

            return View(posts);
        }



        // GET: PostController
        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser();
            var posts = _postRepository.GetAll();
            return View(posts);
        }



        // GET: PostController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postRepository.GetById(id);
            return View(post);
        }



        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
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

                        //Claim userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        //string userId = userIdClaim.Value;   // Get the value

                        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        post.UserId = userId;
                        post.PublishDate = DateTime.Now;
                        //ApplicationUser user = _userManager.Users.Where(a => a.Id == userId).SingleOrDefault();

                        _postRepository.Add(post, image);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(post);

        }




        // GET: PostController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
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
                ModelState.AddModelError("", "Sorry You can't Edit this post , Because it is not yours");
            }
            TempData["oldImage"] = post.Image;
            return View(post);
        }

        // POST: PostController/Edit/5
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

                    Claim userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    string userId = userIdClaim.Value;   // Get the value

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








        // GET: PostController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == (ClaimTypes.NameIdentifier)).Value;

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

        // POST: PostController/Delete/5
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








        public IActionResult ShowComments(int postId)
        {
            var post = _postRepository.GetAllComments(postId);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }














    }
}