using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Services.Post
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Blog.Data.Models.Post>> GetPostsAsync()
        {
            return await _context.Posts.Include(p => p.Autor)
                .Include(p => p.Comentarios)
                .ToListAsync();
        }

        public async Task<Blog.Data.Models.Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddPostAsync(Blog.Data.Models.Post post)
        {
            var user = await GetCurrentUserAsync(); 
            post.UsuarioId = user?.Id; 
            post.DataPostagem = DateTime.Now; 

            _context.Add(post); 
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Blog.Data.Models.Post post)
        {
            post.DataAlteracaoPostagem = DateTime.Now;
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
    }
}