namespace Forum.Models
{
    public class ReplyToComment : ParentForUsers
    {
        public int Id { get; set; }
        public int CommentId { get; set; }





        public virtual Comment Comment { get; set; }
       
        public virtual ICollection<LikeReplyToComment> Likes { get; set; }

    }
}
