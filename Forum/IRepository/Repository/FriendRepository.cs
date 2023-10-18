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
        public void AddRequest(FriendRequest request)
        {
            _context.FriendRequests.Add(request);
            _context.SaveChanges();
        }

        // Cancel Request
        public void CancelRequest(string userId, string currentUserId)
        {
           FriendRequest request =  _context.FriendRequests.Where(a => a.SenderId == currentUserId && a.RecieverId == userId).SingleOrDefault();
            _context.FriendRequests.Remove(request);
            _context.SaveChanges();
        }

        // Get Request By Id
        public FriendRequest GetRequestById(int id)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.Id == id).FirstOrDefault();
            return request;
        }


        // Get Request By User Id and Current User Id
<<<<<<< HEAD
        public FriendRequest CheckRequest(string userId, string currentUserId)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.RecieverId == userId && a.SenderId == currentUserId).FirstOrDefault();
=======
        public FriendRequest GetRequestByUserId(string userId, string currentUserId)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.RecieverId == userId && a .SenderId == currentUserId).FirstOrDefault();
>>>>>>> f9be6153bbe49c4f22427ee2e7852472cf83b471
            return request;
        }




        // Accept Request
        public void AcceptRequest(Friend friend)
        {
            _context.Friends.Add(friend);
            _context.SaveChanges();
        }


        // Reject Request
        public void RejectRequest(FriendRequest request)
        {
            _context.FriendRequests.Remove(request);
            _context.SaveChanges();
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




        // My Friends
        public ICollection<Friend> MyFriends(string currentUserId)
        {
            var friends = _context.Friends.Where(a => a.UserOneId == currentUserId || a .UserTwoId == currentUserId).Include(a => a.UserOne).Include( a => a.UserTwo).ToList();
            return friends;
        }




        // UnFriend or Delete Friend
        public void DeleteFriend(Friend friend)
        {
            _context.Friends.Remove(friend);
            _context.SaveChanges();
        }


        // Get Friend by id
        public Friend GetFriendById(int id)
        {
            return _context.Friends.SingleOrDefault(f => f.Id == id);
        }







    }
}
