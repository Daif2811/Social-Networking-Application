using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly ForumContext _context;

        public FriendRepository(ForumContext context)
        {
            _context = context;
        }

        // Add Request
        public async Task AddRequest(FriendRequest request)
        {
            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();
        }


        // Cancel Request
        public async Task CancelRequest(string userId, string currentUserId)
        {
           FriendRequest request =  _context.FriendRequests.Where(a => a.SenderId == currentUserId && a.RecieverId == userId).SingleOrDefault();
            _context.FriendRequests.Remove(request);
            await _context.SaveChangesAsync();
        }


        // Get Request By Id
        public FriendRequest GetRequestById(int id)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.Id == id).FirstOrDefault();
            return request;
        }


        // Get Request By User Id and Current User Id

        public FriendRequest CheckRequest(string userId, string currentUserId)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.RecieverId == userId && a.SenderId == currentUserId).FirstOrDefault();
            return request;
        }


        // Accept Request
        public async Task AcceptRequest(Friend friend)
        {
            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();
        }


        // Reject Request
        public async Task RejectRequest(FriendRequest request)
        {
            _context.FriendRequests.Remove(request);
            await _context.SaveChangesAsync();
        }



        // All Requests
        public ICollection<FriendRequest> FriendRequests(string currentUserId)
        {
           var requsts =  _context.FriendRequests.Where(a => a.RecieverId == currentUserId).Include(a => a.Sender).ToList();
           return requsts;
        }


        public ICollection<FriendRequest> MyFriendRequests(string currentUserId)
        {
           var requsts =  _context.FriendRequests.Where(a => a.SenderId == currentUserId).Include(a => a.Reciever).ToList();
           return requsts;
        }



        public Friend CheckFriend(string userId, string currentUserId)
        {
            var friend = _context.Friends.SingleOrDefault(a => (a.UserOneId == userId && a.UserTwoId == currentUserId) || (a.UserTwoId == userId && a.UserOneId == currentUserId));
            return friend;
        }
         public bool CheckIfFriend(string userId, string currentUserId)
        {
            var friend = _context.Friends.Any(a => (a.UserOneId == userId && a.UserTwoId == currentUserId) || (a.UserTwoId == userId && a.UserOneId == currentUserId));
            return friend;
        }




        // My Friends
        public ICollection<Friend> MyFriends(string currentUserId)
        {
            var friends = _context.Friends.Where(a => a.UserOneId == currentUserId || a .UserTwoId == currentUserId).Include(a => a.UserOne).Include( a => a.UserTwo).ToList();
            return friends;
        }




        // UnFriend or Delete Friend
        public async Task DeleteFriend(Friend friend)
        {
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }


        // Get Friend by id
        public Friend GetFriendById(int id)
        {
            return _context.Friends.SingleOrDefault(f => f.Id == id);
        }







    }
}
