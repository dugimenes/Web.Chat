namespace Blog.Web.Services.Post
{
    public interface IPostService
    {
        Task<IEnumerable<Blog.Data.Models.Post>> GetPostsAsync();
        Task<Blog.Data.Models.Post> GetPostByIdAsync(int id);
        Task AddPostAsync(Blog.Data.Models.Post post);
        Task UpdatePostAsync(Blog.Data.Models.Post post);
        Task DeletePostAsync(int id);
        bool PostExists(int id);
    }
}
