using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController( IMessageRepository messageRepository, UserManager<ApplicationUser> userManager)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
        }

        public ApplicationUser CurrentUser()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _userManager.FindByIdAsync(currentUserId).Result;

            return user;
        }


        //// Send message
        //public async Task<IActionResult> SendMessage(Message message)
        //{
        //    message.SenderId = CurrentUser().Id;
        //    message.Read = true;
        //    message.Show = true;
        //    message.SendDate = DateTime.Now;
          
        //    await _messageRepository.Add(message);
        //    return Ok();
        //}

       


    }
}
