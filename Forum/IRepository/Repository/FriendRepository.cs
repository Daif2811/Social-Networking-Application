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

        // Request
        public void Request(FriendRequest request)
        {
            _context.FriendRequests.Add(request);
            _context.SaveChanges();
        }

        public FriendRequest GetRequestById(int id)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.Id == id).FirstOrDefault();
            return request;
        }
        public FriendRequest GetRequestByUserId(string userId, string currentUserId)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.RecieverId == userId && a .SenderId == currentUserId).FirstOrDefault();
            return request;
        }

        public void CancelRequest(string userId, string currentUserId)
        {
           FriendRequest request =  _context.FriendRequests.Where(a => a.SenderId == currentUserId && a.RecieverId == userId).SingleOrDefault();
            _context.FriendRequests.Remove(request);
            _context.SaveChanges();
        }



        // Cancel Request
        public void RejectRequest(FriendRequest request)
        {
            _context.FriendRequests.Remove(request);
            _context.SaveChanges();
        }


        // Accept Request
        public void AcceptRequest(Friend friend)
        {
            _context.Friends.Add(friend);
            _context.SaveChanges();
        }

        // All Requests
        public ICollection<FriendRequest> FriendRequests(string userId)
        {
           var requsts =  _context.FriendRequests.Where(a => a.RecieverId == userId).Include(a => a.Sender).ToList();
           return requsts;
        }
        public ICollection<FriendRequest> MyFriendRequests(string userId)
        {
           var requsts =  _context.FriendRequests.Where(a => a.SenderId == userId).Include(a => a.Reciever).ToList();
           return requsts;
        }



        public Friend CheckFriend(string userId, string currentUserId)
        {
            var friend = _context.Friends.SingleOrDefault(a => (a.UserOneId == userId && a.UserTwoId == currentUserId) || (a.UserTwoId == userId && a.UserOneId == currentUserId));
            return friend;
        }




        // My Friends
        public ICollection<Friend> MyFriends(string userId)
        {
            var friends = _context.Friends.Where(a => a.UserOneId == userId || a .UserTwoId == userId).Include(a => a.UserOne).Include( a => a.UserTwo).ToList();
            return friends;
        }




        // Delete Friend
        public void DeleteFriend(Friend friend)
        {
            _context.Friends.Remove(friend);
            _context.SaveChanges();
        }

        public Friend GetFriendById(int id)
        {
            return _context.Friends.SingleOrDefault(f => f.Id == id);
        }







    }
}
