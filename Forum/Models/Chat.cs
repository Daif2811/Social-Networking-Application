namespace Forum.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CurrentUserId { get; set; }



        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser CurrentUser { get; set; }
        public virtual ICollection<Message> Messages { get; set; }



    }
}
