namespace Forum.Models
{
    public class Follow
    {
        public int Id { get; set; }

        public string FollowerId { get; set; }

        public string FollowedId { get; set; }

        public DateTime FollowDate { get; set; }
        public virtual ApplicationUser Follower { get; set; }
        public virtual ApplicationUser Followed { get; set; }
    }
}
