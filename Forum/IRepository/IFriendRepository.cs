using Forum.Models;

namespace Forum.IRepository
{
    public interface IFriendRepository
    {

        // All Requests
        ICollection<FriendRequest> FriendRequests(string currentUserId);
        ICollection<FriendRequest> MyFriendRequests(string currentUserId);

        // Add and Cancel Request
        Task AddRequest(FriendRequest request);
        Task CancelRequest(string userId, string currentUserId);

        FriendRequest GetRequestById(int id);
        FriendRequest CheckRequest(string userId, string currentUserId);



        // Accept and Reject Request
        Task AcceptRequest(Friend friend);
        Task RejectRequest(FriendRequest request);



        // Friends  and  check fiend  and (Delete) Unfriend
        ICollection<Friend> MyFriends(string userId);
        Friend CheckFriend(string userId, string currentUserId);
        bool CheckIfFriend(string userId, string currentUserId);
        Task DeleteFriend(Friend friend);
        Friend GetFriendById(int id);



        


    }



}
