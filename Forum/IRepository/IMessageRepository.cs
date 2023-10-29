using Forum.Models;

namespace Forum.IRepository
{
    public interface IMessageRepository
    {
        ICollection<Message> GetAll();
        Message GetById(int id);
        ICollection<Message> GetByChatId(int chatId);


        Task Add (Message message);
        Task Update (Message message);
        Task Delete (int id);




    }
}
