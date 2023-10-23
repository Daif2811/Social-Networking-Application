using Forum.DAL;
using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Forum.Custom_Attributes
{
    public class CheckIfBlockedByAdminAttribute : ActionFilterAttribute
    {

        private readonly ForumContext _context;

        public CheckIfBlockedByAdminAttribute(ForumContext context)
        {
            _context = context;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user is blocked by an admin
            var isUserBlocked = _context.BlocksByAdmins.Any(u => u.UserId == userId);

            if (isUserBlocked)
            {
                context.Result = new RedirectToActionResult("BlockedPage", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
