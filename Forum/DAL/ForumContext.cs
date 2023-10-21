using Forum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Forum.DAL
{
    public class ForumContext : IdentityDbContext<ApplicationUser>
    {

        public ForumContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReplyToComment> ReplyToComments { get; set; }


        public DbSet<BlockByAdmin> BlocksByAdmins { get; set; }
        public DbSet<BlockByUser> BlocksByUsers { get; set; }
        
        
        
        public DbSet<PostReport> PostReports { get; set; }
        public DbSet<UserReport> UserReports { get; set; }




        public DbSet<LikePost> LikePosts { get; set; }
        public DbSet<LikeComment> LikeComments { get; set; }
        public DbSet<LikeReplyToComment> LikeReplyToComments { get; set; }
       

        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>().HasIndex(p => p.UserId);
            builder.Entity<Post>().HasIndex(p => p.PublishDate);
            builder.Entity<Post>().HasIndex(p => new { p.UserId, p.PublishDate });
            builder.Entity<Comment>().HasIndex(p => p.UserId);
            builder.Entity<Comment>().HasIndex(p => p.PostId);
            builder.Entity<Comment>().HasIndex(p => p.PublishDate);
            builder.Entity<Comment>().HasIndex(p => new { p.UserId, p.PublishDate });
            builder.Entity<ReplyToComment>().HasIndex(p => p.UserId);
            builder.Entity<ReplyToComment>().HasIndex(p => p.CommentId);
            builder.Entity<ReplyToComment>().HasIndex(p => p.PublishDate);
            builder.Entity<ReplyToComment>().HasIndex(p => new { p.UserId, p.PublishDate });
          


            //builder.Entity<RegisterViewModel>().HasNoKey();
            base.OnModelCreating(builder);
        }
    }
}
