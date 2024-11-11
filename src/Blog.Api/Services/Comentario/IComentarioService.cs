using Blog.Api.Request;

namespace Blog.Api.Services.Comentario
{
    public interface IComentarioService
    {
        Task<IEnumerable<Data.Models.Comentario>> ObterComentariosAsync();
        Task<Data.Models.Comentario> ObterComentarioPorIdAsync(int id);
        Task<Data.Models.Comentario> CadastrarComentarioAsync(ComentarioRequest comentarioRequest, int? userId);
        Task AtualizarComentarioAsync(int id, Data.Models.Comentario comentario);
        Task RemoverComentarioAsync(int id);
    }
}