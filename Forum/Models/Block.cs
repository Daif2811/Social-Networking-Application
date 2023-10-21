namespace Forum.Models
{
    public class Block
    {
        public string UserId { get; set; }

        public DateTime BlockDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }


    public class BlockByAdmin : Block
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public virtual ApplicationUser Admin { get; set; }
    }


    public class BlockByUser : Block
    {
        public int Id { get; set; }
        public string BlockerId { get; set; }
        public virtual ApplicationUser Blocker { get; set; }
    }

}
