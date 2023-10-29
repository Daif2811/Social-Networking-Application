namespace Forum.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }

        public string SenderId { get; set; }
        public int ChatId { get; set; }



        public bool Read { get; set; }
        public bool Show { get; set; }



        public virtual  ApplicationUser Sender { get; set; }
        public virtual Chat Chat { get; set; }






    }
}
