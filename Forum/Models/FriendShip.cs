namespace Forum.Models
{

    public class FriendRequest
    {
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string RecieverId { get; set; }

    


        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Reciever { get; set; }


    }
    public class Friend
    {
        public int Id { get; set; }

        public string UserOneId { get; set; }

        public string UserTwoId { get; set; }

        public virtual ApplicationUser UserOne { get; set; }
        public virtual ApplicationUser UserTwo { get; set; }

    }


}
