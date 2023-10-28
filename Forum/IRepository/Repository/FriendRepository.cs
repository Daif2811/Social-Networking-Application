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
        public async Task CancelRequest(string recieverId, string senderId)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.SenderId == senderId && a.RecieverId == recieverId).SingleOrDefault();
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

        public FriendRequest GetRequestByUsersId(string senderId, string recieverId)
        {
            FriendRequest request = _context.FriendRequests.Where(a => a.RecieverId == recieverId && a.SenderId == senderId).FirstOrDefault();
            return request;
        }
        public bool CheckRequest(string senderId, string recieverId)
        {
            bool request = _context.FriendRequests.Any(a => a.RecieverId == recieverId && a.SenderId == senderId);
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
        public ICollection<FriendRequest> FriendRequests(string recieverId)
        {
            var requsts = _context.FriendRequests.Where(a => a.RecieverId == recieverId).Include(a => a.Sender).ToList();
            return requsts;
        }


        public ICollection<FriendRequest> MyFriendRequests(string senderId)
        {
            var requsts = _context.FriendRequests.Where(a => a.SenderId == senderId).Include(a => a.Reciever).ToList();
            return requsts;
        }



        public bool CheckFriend(string userOneId, string userTwoId)
        {
            var friend = _context.Friends.Any(a => (a.UserOneId == userOneId && a.UserTwoId == userTwoId) || (a.UserTwoId == userOneId && a.UserOneId == userTwoId));
            return friend;
        }

        public Friend GetFriendByUsersId(string userOneId, string userTwoId)
        {
            var friend = _context.Friends.Where(a => (a.UserOneId == userOneId && a.UserTwoId == userTwoId) || (a.UserTwoId == userOneId && a.UserOneId == userTwoId)).SingleOrDefault();
            return friend;
        }




        // My Friends
        public ICollection<Friend> MyFriends(string userId)
        {
            var friends = _context.Friends.Where(a => a.UserOneId == userId || a.UserTwoId == userId).Include(a => a.UserOne).Include(a => a.UserTwo).ToList();
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
