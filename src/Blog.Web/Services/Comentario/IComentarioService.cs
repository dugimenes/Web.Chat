using Blog.Data.Models;

namespace Blog.Web.Services.Comentario
{
    public interface IComentarioService
    {
        Task<IEnumerable<Blog.Data.Models.Comentario>> GetComentariosAsync(int? userId);
        Task<Blog.Data.Models.Comentario> GetComentarioByIdAsync(int id);
        Task AddComentarioAsync(Blog.Data.Models.Comentario comentario, ApplicationUser user);
        Task UpdateComentarioAsync(Blog.Data.Models.Comentario comentario);
        Task DeleteComentarioAsync(int id);
        bool ComentarioExists(int id);
        Task<bool> EhAdminAsync(ApplicationUser user, int? ownerId);
    }
}