using Forum.IRepository;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Forum.Hubs
{
    [Authorize]
    public class chatHub : Hub
    {

        private readonly IMessageRepository _messageRepository;

        public chatHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        
        public async Task SendMessage( int chatId, string message)
        {
            // string currentUserId = Context.UserIdentifier;
            var currentUserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            string senderConnectionId = Context.ConnectionId;

            Message newMessage = new Message()
            {
                SenderId = currentUserId,
                Read = true,
                Show = true,
                SendDate = DateTime.Now,
                ChatId = chatId,
                Content = message
            };


            await _messageRepository.Add(newMessage);



            await Clients.All.SendAsync("ReceiveMessage",message, senderConnectionId);
        }

        //[Authorize]
        //public override async Task OnConnectedAsync()
        //{
        //    string user = Context.User.Identity.Name;
        //    //enter code here to keep track of connected clients
        //    var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier); // get the username of the connected user

        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

        //    await base.OnConnectedAsync();
        //}


    }
}