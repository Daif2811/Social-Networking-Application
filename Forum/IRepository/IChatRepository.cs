using Forum.Models;

namespace Forum.IRepository
{
    public interface IChatRepository
    {

        ICollection<Chat> GetAll();
        Chat GetById(int id);
        Chat GetByUsersId(string userOne, string userTwo);
        ICollection<Chat> GetByUserId(string currentUserId);
        bool CheckChat (string userOne, string userTwo);

        Task Add(Chat chat);
        Task Update(Chat chat);
        Task Delete(int id);


    }
}
