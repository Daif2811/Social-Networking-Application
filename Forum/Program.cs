using Forum.DAL;
using Forum.Hubs;
using Forum.IRepository;
using Forum.IRepository.Repository;
using Forum.IService;
using Forum.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ForumContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });


            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.SignIn.RequireConfirmedAccount = true;
            }).AddEntityFrameworkStores<ForumContext>();


            // Add SignalR
            builder.Services.AddSignalR();

            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("SignalR", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                   .WithOrigins("http://localhost:56806/Chat/");  // Add your client-side URL;
                });
            });



            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 50000000;
            });


            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IReplyToCommentRepository, ReplyToCommentRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<IFriendRepository, FriendRepository>();
            builder.Services.AddScoped<IPostReportRepository, PostReportRepository>();
            builder.Services.AddScoped<IUserReportRepository, UserReportRepository>();
            builder.Services.AddScoped<IBlockByAdminRepository, BlockByAdminRepository>();
            builder.Services.AddScoped<IBlockByUserRepository, BlockByUserRepository>();
            builder.Services.AddScoped<IFollowRepository, FollowRepository>();
            builder.Services.AddScoped<ISavePostRepository, SavePostRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();


            // Use Cors for allow another domains to use my service
            app.UseCors("SignalR");



            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            // Add signalR
            app.MapHub<chatHub>("/chatHub");
            app.Run();
        }
    }
}