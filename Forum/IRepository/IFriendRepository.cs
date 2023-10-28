using Forum.Models;

namespace Forum.IRepository
{
    public interface IFriendRepository
    {

        // All Requests
        ICollection<FriendRequest> FriendRequests(string recieverId);
        ICollection<FriendRequest> MyFriendRequests(string senderId);

        // Add and Cancel Request
        Task AddRequest(FriendRequest request);
        Task CancelRequest(string recieverId, string senderId);

        FriendRequest GetRequestById(int id);
        bool CheckRequest(string senderId, string recieverId);
        FriendRequest GetRequestByUsersId(string senderId, string recieverId);



        // Accept and Reject Request
        Task AcceptRequest(Friend friend);
        Task RejectRequest(FriendRequest request);



        // Friends  and  check fiend  and (Delete) Unfriend
        ICollection<Friend> MyFriends(string userId);
        bool CheckFriend(string UserOneId, string UserTwoId);
        Friend GetFriendByUsersId(string UserOneId, string UserTwoId);

        Task DeleteFriend(Friend friend);
        Friend GetFriendById(int id);






    }



}
