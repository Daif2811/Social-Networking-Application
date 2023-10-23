using Forum.Models;

namespace Forum.IRepository
{
    public interface IBlockByAdminRepository
    {
        ICollection<BlockByAdmin> GetAll();
        ICollection<BlockByAdmin> GetAllByAdmin(string adminId);
        BlockByAdmin GetById(int id);
        BlockByAdmin GetByUserId(string userId);
        bool CheckBlock(string userId);
        Task Add (BlockByAdmin blockByAdmin);
        Task Update (BlockByAdmin blockByAdmin);
        Task Delete (int id);
    }
    
    
    public interface IBlockByUserRepository
    {
        ICollection <BlockByUser> GetAll();
        ICollection <BlockByUser> GetAllByUser(string currentUserId);
        BlockByUser GetById (int id);
        BlockByUser GetByUserId(string userId);
        bool CheckBlock(string currentUserId , string userId);
        Task Add (BlockByUser blockByUser);
        Task Update (BlockByUser blockByUser);
        Task Delete (int id);

    }
}
