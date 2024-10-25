using Blog.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Services
{
    public class UserService<TUser> : IUserService where TUser : ApplicationUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<TUser> _userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<TUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<int?> GetUserIdAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
                return null;

            var user = await _userManager.FindByNameAsync(userName);
            return user?.Id;
        }

        public async Task<string> GetUserNameAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
                return "Desconhecido";

            var user = await _userManager.FindByNameAsync(userName);
            return user?.UserName ?? "Usuário não encontrado";
        }
    }
}