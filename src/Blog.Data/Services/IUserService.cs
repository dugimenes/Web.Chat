namespace Blog.Data.Services
{
    public interface IUserService
    {
        Task<int?> GetUserIdAsync();
        Task<string> GetUserNameAsync();
    }
}