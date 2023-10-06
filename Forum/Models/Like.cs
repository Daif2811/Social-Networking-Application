namespace Forum.Models
{
    public class LikePost
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        




        public virtual ApplicationUser User { get; set; }

        public virtual Post Post { get; set; }
        
    }


    public class LikeComment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }





        public virtual ApplicationUser User { get; set; }

        public virtual Comment Comment { get; set; }

    }


    public class LikeReplyToComment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ReplyId { get; set; }



        public virtual ApplicationUser User { get; set; }

        public virtual ReplyToComment Reply { get; set; }

    }



   

}
