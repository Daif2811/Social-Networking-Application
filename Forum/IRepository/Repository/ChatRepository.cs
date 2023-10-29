using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ForumContext _context;

        public ChatRepository(ForumContext context)
        {
            _context = context;
        }


        public ICollection<Chat> GetAll()
        {
            return _context.Chats.ToList();
        }



        public ICollection<Chat> GetByUserId(string currentUserId)
        {
            return _context.Chats.Where(c => c.UserId == currentUserId || c.CurrentUserId == currentUserId)
                .Include(c => c.User).Include(c => c.CurrentUser).Include(c => c.Messages).ThenInclude(m => m.Sender)
                .ToList();

        }


        public Chat GetById(int id)
        {
            return _context.Chats.FirstOrDefault(c => c.Id == id);
        }

        public Chat GetByUsersId(string userOne, string userTwo)
        {
           return _context.Chats.Where(c => (c.UserId == userOne && c.CurrentUserId == userTwo) || (c.CurrentUserId == userOne && c.UserId == userTwo))
                .Include(c => c.User).Include(c => c.CurrentUser).Include(c => c.Messages).ThenInclude(m => m.Sender)
                .FirstOrDefault();
        }

        public async Task Add(Chat chat)
        {
           _context.Chats.Add(chat);
           await _context.SaveChangesAsync();
        }
        public async Task Update(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Chat chat = GetById(id);
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }




        public bool CheckChat(string userOne, string userTwo)
        {
            bool found = _context.Chats.Any(c => (c.UserId == userOne && c.CurrentUserId == userTwo) || (c.CurrentUserId == userOne && c.UserId == userTwo));
            return found;
        }
    }
}
