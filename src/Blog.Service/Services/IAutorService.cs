using Blog.Data.Models;

namespace Blog.Services.Services
{
    public interface IAutorService
    {
        Task CreateAutorAsync(ApplicationUser user);
        Task UpdateAutorAsync(ApplicationUser user);
    }
}