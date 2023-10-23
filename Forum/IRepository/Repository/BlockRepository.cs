using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class BlockByAdminRepository : IBlockByAdminRepository
    {
        private readonly ForumContext _context;

        public BlockByAdminRepository(ForumContext context)
        {
            _context = context;
        }



        public ICollection<BlockByAdmin> GetAll()
        {
          var blocks =  _context.BlocksByAdmins.Include(a => a.User).ToList();
            return blocks;
        }

        public ICollection<BlockByAdmin> GetAllByAdmin(string adminId)
        {
            return _context.BlocksByAdmins.Where(a => a.AdminId == adminId).ToList();
        }
        public BlockByAdmin GetById(int id)
        {
            return _context.BlocksByAdmins.FirstOrDefault(a => a.Id == id);
        }
        
        public BlockByAdmin GetByUserId(string userId)
        {
            return _context.BlocksByAdmins.FirstOrDefault(a => a.UserId == userId);
        }
        
        public bool CheckBlock(string userId)
        {
            return _context.BlocksByAdmins.Any(a => a.UserId == userId);
        }
        public void Add(BlockByAdmin blockByAdmin)
        {
           _context.BlocksByAdmins.Add(blockByAdmin);
           _context.SaveChanges();
        }
        public void Update(BlockByAdmin blockByAdmin)
        {
            _context.BlocksByAdmins.Update(blockByAdmin);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
           BlockByAdmin block = GetById(id);
            _context.BlocksByAdmins.Remove(block);
            _context.SaveChanges();
        }



    }


    public class BlockByUserRepository : IBlockByUserRepository
    {

        private readonly ForumContext _context;

        public BlockByUserRepository(ForumContext context)
        {
            _context = context;
        }

        public ICollection<BlockByUser> GetAll()
        {
            return _context.BlocksByUsers.Include(a => a.User).ToList();

        }

        public ICollection<BlockByUser> GetAllByUser(string currentUserId)
        {
            return _context.BlocksByUsers.Where(a => a.BlockerId == currentUserId).Include(a => a.User).ToList();
        }
        public BlockByUser GetById(int id)
        {
            return _context.BlocksByUsers.FirstOrDefault(a => a.Id == id);
        }
        public BlockByUser GetByUserId(string userId)
        {
            return _context.BlocksByUsers.FirstOrDefault(a => a.UserId == userId);
        }
        public bool CheckBlock(string currentUserId, string userId)
        {
            return _context.BlocksByUsers.Any (a => a.UserId == currentUserId && a.BlockerId == userId);
        }

        public void Add(BlockByUser blockByUser)
        {
            _context.BlocksByUsers.Add(blockByUser);
            _context.SaveChanges();
        }
        public void Update(BlockByUser blockByUser)
        {
            _context.BlocksByUsers.Update(blockByUser);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            BlockByUser block = GetById(id);
            _context.BlocksByUsers.Remove(block);
            _context.SaveChanges();
        }
    }
}
