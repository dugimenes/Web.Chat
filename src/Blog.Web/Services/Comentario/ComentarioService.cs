using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Services.Comentario
{
    public class ComentarioService : IComentarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComentarioService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Blog.Data.Models.Comentario>> GetComentariosAsync(int? userId)
        {
            var comentarios = _context.Comentarios
                .Include(c => c.Autor)
                .Include(c => c.Post);

            if (userId.HasValue)
            {
                var user = await GetCurrentUserAsync();
                if (await EhAdminAsync(user, userId))
                {
                    return await comentarios.ToListAsync();
                }
                else
                {
                    return await comentarios.Where(x => x.UsuarioId == userId).ToListAsync();
                }
            }

            return await comentarios.ToListAsync();
        }

        public async Task<Blog.Data.Models.Comentario> GetComentarioByIdAsync(int id)
        {
            return await _context.Comentarios
                .Include(c => c.Autor)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddComentarioAsync(Blog.Data.Models.Comentario comentario, ApplicationUser user)
        {
            comentario.UsuarioId = user?.Id;
            comentario.DataCadastro = DateTime.Now;

            _context.Add(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComentarioAsync(Blog.Data.Models.Comentario comentario)
        {
            _context.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComentarioAsync(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
            }
        }

        public bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.Id == id);
        }

        public async Task<bool> EhAdminAsync(ApplicationUser user, int? ownerId)
        {
            return await _userManager.IsInRoleAsync(user, "Admin") || ownerId == user.Id;
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
    }
}