using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services.Autor
{
    public class AutorService : IAutorService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AutorService> _logger;

        public AutorService(ApplicationDbContext context, ILogger<AutorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Data.Models.Autor>> ObterAutoresAsync()
        {
            return await _context.Autores.ToListAsync();
        }

        public async Task<Data.Models.Autor> ObterAutorPorIdAsync(int id)
        {
            return await _context.Autores.FindAsync(id);
        }

        public async Task AtualizarAutorAsync(int id, Data.Models.Autor autor)
        {
            if (id != autor.Id)
            {
                throw new ArgumentException("ID inválido para atualização.");
            }

            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAutorAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Autor não encontrado.");
            }
        }

        public async Task CreateAutorAsync(ApplicationUser user, RegisterUserViewModel model)
        {
            var autor = new Data.Models.Autor
            {
                Id = user.Id,
                Nome = model.Nome,
                SobreNome = model.Sobrenome,
                DataCadastro = DateTime.Now,
                User = user
            };

            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Autor criado para o usuário: {UserId}", user.Id);
        }

        public async Task UpdateAutorAsync(ApplicationUser user)
        {
            var autor = await _context.Autores.FindAsync(user.Id);
            if (autor != null)
            {
                autor.Nome = user.UserName;
                autor.SobreNome = user.UserName;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Autor atualizado para o usuário: {UserId}", user.Id);
            }
        }
    }
}