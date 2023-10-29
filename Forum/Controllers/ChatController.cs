using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(IChatRepository chatRepository, UserManager<ApplicationUser> userManager)
        {
            _chatRepository = chatRepository;
            _userManager = userManager;
        }


        public ApplicationUser CurrentUser()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _userManager.FindByIdAsync(currentUserId).Result;
            return user;
        }


        // All chats
        public IActionResult AllChats()
        {
            ApplicationUser currentUser = CurrentUser();
            var chats = _chatRepository.GetByUserId(currentUser.Id);

            ViewBag.CurrentUserId = currentUser.Id;

            return View(chats);
        }


        // Open Chat
        public IActionResult OpenChat(string userId)
        {
            ApplicationUser currentUser = CurrentUser();
           
            bool found = _chatRepository.CheckChat(currentUser.Id, userId);
            if (found)
            {
                var chat = _chatRepository.GetByUsersId(currentUser.Id, userId);
                if (chat != null)
                {
                    ViewBag.CurrentUserId = currentUser.Id;
                    return View(chat);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return RedirectToAction("CreateChat", new { userId });
            }
        }



        // New Chat
        public async Task<IActionResult> CreateChat(string userId)
        {
            ApplicationUser currentUser = CurrentUser();

            Chat newChat = new Chat()
            {
                CurrentUserId = currentUser.Id,
                UserId = userId
            };

            await _chatRepository.Add(newChat);

            return RedirectToAction("OpenChat", new { userId });
        }


    }
}
