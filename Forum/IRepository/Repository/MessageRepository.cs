using Forum.DAL;
using Forum.Models;

namespace Forum.IRepository.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ForumContext _context;

        public MessageRepository(ForumContext context)
        {
            _context = context;
        }

        public ICollection<Message> GetAll()
        {
            return _context.Messages.ToList();
        }

        public ICollection<Message> GetByChatId(int chatId)
        {
            return _context.Messages.Where(m => m.ChatId == chatId).ToList(); ;
        }

        public Message GetById(int id)
        {
            return _context.Messages.FirstOrDefault(m => m.Id == id);
        }

        public async Task Add(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Message message = GetById(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
