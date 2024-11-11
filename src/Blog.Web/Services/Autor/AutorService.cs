using Blog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Services.Autor
{
    public class AutorService : IAutorService
    {
        private readonly ApplicationDbContext _context;

        public AutorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog.Data.Models.Autor>> GetAutoresAsync()
        {
            return await _context.Autores.ToListAsync();
        }

        public async Task<Blog.Data.Models.Autor> GetAutorByIdAsync(int id)
        {
            return await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAutorAsync(Blog.Data.Models.Autor autor)
        {
            _context.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAutorAsync(Blog.Data.Models.Autor autor)
        {
            _context.Update(autor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAutorAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }
        }

        public bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.Id == id);
        }
    }
}