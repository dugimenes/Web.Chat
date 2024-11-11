using Blog.Api.Request;
using Blog.Data.Models;

namespace Blog.Api.Services.Postagem
{
    public interface IPostagemService
    {
        Task<IEnumerable<Post>> ObterPostsAsync();
        Task<Post> ObterPostPorIdAsync(int id);
        Task<Post> CadastrarPostAsync(PostRequest postagem);
        Task AtualizarPostAsync(int id, Post postagem);
        Task RemoverPostAsync(int id);
    }

}