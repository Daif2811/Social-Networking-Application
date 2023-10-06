using Forum.Models;

namespace Forum.IRepository
{
    public interface IFriendRepository
    {

        // Request
        ICollection<FriendRequest> FriendRequests(string userId);
        ICollection<FriendRequest> MyFriendRequests(string userId);
        void Request(FriendRequest request);
        void CancelRequest(string userId, string currentUserId);
        FriendRequest GetRequestById(int id);
        FriendRequest GetRequestByUserId(string userId, string currentUserId);



        // Accept
        void AcceptRequest(Friend friend);
        void RejectRequest(FriendRequest request);



        // Friends
        Friend CheckFriend(string userId, string currentUserId);
        ICollection<Friend> MyFriends(string userId);
        void DeleteFriend(Friend friend);
        Friend GetFriendById(int id);



        


    }



}
