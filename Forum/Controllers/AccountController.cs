using Forum.DAL;
using Forum.IService;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IAdminService _adminService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment environment,
            IAdminService adminService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _adminService = adminService;
        }



        // Get Current User
        [HttpGet]
        public ApplicationUser CurrentUser()
        {
            //// Way to get UserId
            ////Claim userClaim = User.Claims.SingleOrDefault(a => a.Type == (ClaimTypes.NameIdentifier));
            ////string userId = userClaim.Value;


            // another way to get userId
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
            return currentUser;
        }




        // Dashboard For Users
        [HttpGet]
        public IActionResult Setting()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDash()
        {
            ViewBag.CurrentAdmin = CurrentUser();
            AdminViewModel adminViewModel = new AdminViewModel()
            {
                Users = _userManager.Users.Include(a => a.Posts).ToList(),
                Messages = _adminService.Messages().ToList(),
                BlocksByAdmin = _adminService.AdminBlocks().ToList(),
                BlocksByUser = _adminService.UserBlocks().ToList(),
                LikePosts = _adminService.LikePosts().ToList(),
                Posts = _adminService.Posts().ToList(),
                UserReports = _adminService.UserReports().ToList(),
                PostReports = _adminService.PostReports().ToList(),
                Chats = _adminService.chats().ToList(),
            };

            return View(adminViewModel);
        }



        // All Users
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }



        // GET: Account/Register
        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }




        // POST: Account/Register
        [HttpPost, AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new user account with the image file name
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Summary = model.Summary,
                    UserType = "User",
                    PasswordHash = model.Password,

                };

                if (model.Picture != null)
                {

                    // Save the image file to the wwwroot/images folder
                    var fileName = Path.GetFileName(model.Picture.FileName);
                    var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Picture.CopyToAsync(fileStream);
                    }
                    user.Picture = fileName;
                }

                // Add the user to the database using the user manager
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //Sign in the user and redirect to the home page
                    await _signInManager.SignInAsync(user, isPersistent: false);


                    IdentityResult AddToRole = await _userManager.AddToRoleAsync(user, "User");
                    if (AddToRole.Succeeded)
                    {
                        return RedirectToAction("Index", "Post");
                    }
                    foreach (var error in AddToRole.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }

                // Add any errors to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Return the view with the model
            return View(model);
        }



        // Get: Account/AddUser
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            return View();
        }



        // Post: Account/AddUser
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    IdentityResult AddToRole = await _userManager.AddToRoleAsync(user, "User");
                    if (AddToRole.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in AddToRole.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);

        }



        // GET: Account/login
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        // POST: Account/login
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var found = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(user, model.RememberMe);
                        return RedirectToAction("Index", "Post");
                    }
                    ModelState.AddModelError("", "Your password is wrong");
                }
                else
                {
                    ModelState.AddModelError("", "User Name is wrong");
                }


            }
            return View(model);
        }





        // POST: Account/logOut
        [HttpGet]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }




        // GET: Account/Editprofile
        [HttpGet]
        public IActionResult EditProfile()
        {
            ApplicationUser user = CurrentUser();



            EditProfileViewModel profile = new EditProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                UserType = "User",
                Summary = user.Summary,
                Picture = user.Picture,
                PhoneNumber = user.PhoneNumber,
            };

            return View(profile);
        }




        // POST: Account/Editprofile
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, IFormFile picture)
        {
            ApplicationUser currentUser = CurrentUser();

            if (ModelState.IsValid)
            {
                bool checkPass = await _userManager.CheckPasswordAsync(currentUser, model.Password);
                if (checkPass)
                {
                    // Add picture
                    if (picture != null)
                    {
                        // delete the old picture
                        if (currentUser.Picture != null)
                        {
                            var oldPath = Path.Combine(_environment.WebRootPath, "images", currentUser.Picture);
                            System.IO.File.Delete(oldPath);
                        }

                        // save new picture
                        var fileName = Path.GetFileName(picture.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await picture.CopyToAsync(fileStream);
                        }

                        currentUser.Picture = fileName;
                    }

                    // save new data
                    currentUser.UserName = model.UserName;
                    currentUser.Email = model.Email;
                    currentUser.Summary = model.Summary;
                    currentUser.UserType = model.UserType;
                    currentUser.PhoneNumber = model.PhoneNumber;

                    // Save changes()
                    var updated = await _userManager.UpdateAsync(currentUser);
                    if (updated != null)
                    {
                        return RedirectToAction("Users");
                    }
                }
                ModelState.AddModelError("", "Password is wrong");

            }

            return View(model);
        }



        // GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }




        // POST: Account/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ApplicationUser currentUser = CurrentUser();


            if (ModelState.IsValid)
            {
                bool checkPass = await _userManager.CheckPasswordAsync(currentUser, model.CurrentPassword);
                if (checkPass)
                {
                    // Hash new password
                    var newPass = _userManager.PasswordHasher.HashPassword(currentUser, model.NewPassword);
                    // Sava password
                    currentUser.PasswordHash = newPass;

                    // Save changes()
                    var updated = await _userManager.UpdateAsync(currentUser);
                    if (updated != null)
                    {
                        return RedirectToAction("Users");
                    }
                }
                ModelState.AddModelError("", "Sorry, Current Password is wrong");
            }

            return View(model);
        }




        // GET: Account/ChangePicture
        [HttpGet]
        public IActionResult ChangePicture()
        {
            ApplicationUser currentUser = CurrentUser();

            return View(currentUser);
        }




        // POST: Account/ChangePicture
        [HttpPost]
        public async Task<IActionResult> ChangePicture(IFormFile picture, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter your password");
            }

            ApplicationUser currentUser = CurrentUser();


            if (ModelState.IsValid)
            {

                bool checkPass = await _userManager.CheckPasswordAsync(currentUser, password);
                if (checkPass)
                {
                    if (picture != null)
                    {
                        if (currentUser.Picture != null)
                        {
                            // delete the old picture
                            var oldPath = Path.Combine(_environment.WebRootPath, "images", currentUser.Picture);
                            System.IO.File.Delete(oldPath);
                        }

                        // save new picture
                        var fileName = Path.GetFileName(picture.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await picture.CopyToAsync(fileStream);
                        }

                        // save new picture
                        currentUser.Picture = fileName;

                        // save changes()
                        var updated = await _userManager.UpdateAsync(currentUser);
                        if (updated != null)
                        {
                            return RedirectToAction("ChangePicture");
                        }
                    }

                }
                ModelState.AddModelError("", "Sorry, Your Password is wrong");

            }

            return View(currentUser);
        }







        // =======================================================================
        // =======================================================================


        // Roles
        [HttpGet]
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }




        // GET: Account/DetailRole/id
        [HttpGet]
        public async Task<IActionResult> DetailRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }



        // GET: Account/AddRole
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }



        // POST: Account/AddRole
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.Name,
                };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }



        // GET: Account/EditRole/id
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }




        // POST: Account/EditRole/id
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole model)
        {
            if (ModelState.IsValid)
            {

                var result = await _roleManager.UpdateAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }




        // GET: Account/DeleteRole/id
        [HttpGet]
        public async Task<ActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }




        // POST: Account/DeleteRole/id
        [HttpPost, ActionName("DeleteRole")]
        public async Task<ActionResult> ConfirmDeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();

        }



    }
}
