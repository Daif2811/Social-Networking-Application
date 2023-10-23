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
        void Add (BlockByAdmin blockByAdmin);
        void Update (BlockByAdmin blockByAdmin);
        void Delete (int id);
    }
    
    
    public interface IBlockByUserRepository
    {
        ICollection <BlockByUser> GetAll();
        ICollection <BlockByUser> GetAllByUser(string currentUserId);
        BlockByUser GetById (int id);
        BlockByUser GetByUserId(string userId);
        bool CheckBlock(string currentUserId , string userId);
        void Add (BlockByUser blockByUser);
        void Update (BlockByUser blockByUser);
        void Delete (int id);

    }
}
