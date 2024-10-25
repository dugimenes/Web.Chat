using Blog.Data.Models;

namespace Blog.Data.Services
{
    public interface IAutorService
    {
        Task CreateAutorAsync(ApplicationUser user, RegisterUserViewModel autor);
        Task UpdateAutorAsync(ApplicationUser user);
    }
}