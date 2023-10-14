using Forum.Models;

namespace Forum.IRepository
{
    public interface IFriendRepository
    {

        // All Requests
        ICollection<FriendRequest> FriendRequests(string currentUserId);
        ICollection<FriendRequest> MyFriendRequests(string currentUserId);

        // Add and Cancel Request
        void AddRequest(FriendRequest request);
        void CancelRequest(string userId, string currentUserId);

        FriendRequest GetRequestById(int id);
        FriendRequest CheckRequest(string userId, string currentUserId);



        // Accept and Reject Request
        void AcceptRequest(Friend friend);
        void RejectRequest(FriendRequest request);



        // Friends  and  check fiend  and (Delete) Unfriend
        ICollection<Friend> MyFriends(string userId);
        Friend CheckFriend(string userId, string currentUserId);
        void DeleteFriend(Friend friend);
        Friend GetFriendById(int id);



        


    }



}
