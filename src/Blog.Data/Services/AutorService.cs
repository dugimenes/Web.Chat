using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.Extensions.Logging;

namespace Blog.Data.Services
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

        public async Task CreateAutorAsync(ApplicationUser user, RegisterUserViewModel model)
        {
            var autor = new Autor
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