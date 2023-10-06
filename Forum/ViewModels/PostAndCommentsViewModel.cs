using Forum.Models;

namespace Forum.ViewModels
{
    public class PostAndCommentsViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
