using Forum.Models;

namespace Forum.IRepository
{
    public interface IBlockByAdminRepository
    {
        ICollection<BlockByAdmin> GetAll();
        ICollection<BlockByAdmin> GetAllByAdmin(string adminId);
        BlockByAdmin GetById(int id);
        BlockByAdmin GetByUserId(string blockedUserId);
        bool CheckBlock(string blockedUserId);
        Task Add (BlockByAdmin blockByAdmin);
        Task Update (BlockByAdmin blockByAdmin);
        Task Delete (int id);
    }
    
    
    public interface IBlockByUserRepository
    {
        ICollection <BlockByUser> GetAll();
        ICollection <BlockByUser> GetAllByUser(string blockerId);
        BlockByUser GetById (int id);
        BlockByUser GetByUserId(string blockedId);
        bool CheckBlock(string blockedId, string blockerId);
        Task Add (BlockByUser blockByUser);
        Task Update (BlockByUser blockByUser);
        Task Delete (int id);

    }
}
