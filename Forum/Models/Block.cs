namespace Forum.Models
{
    public class Block
    {
        public int Id { get; set; }
        public string AdminId { get; set; }

        public string UserId { get; set; }



        public virtual ApplicationUser Admin { get; set; }
    }
}
