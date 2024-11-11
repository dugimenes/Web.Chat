using Blog.Data.Models;

namespace Blog.Api.Services.Autor
{
    public interface IAutorService
    {
        Task<IEnumerable<Data.Models.Autor>> ObterAutoresAsync();
        Task<Data.Models.Autor> ObterAutorPorIdAsync(int id);
        Task AtualizarAutorAsync(int id, Data.Models.Autor autor);
        Task RemoverAutorAsync(int id);
        Task CreateAutorAsync(ApplicationUser user, RegisterUserViewModel autor);
        Task UpdateAutorAsync(ApplicationUser user);
    }
}