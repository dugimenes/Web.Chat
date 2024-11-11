using Blog.Api.Request;
using Blog.Data.Models;
using Blog.Data.Services;
using Blog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services.Postagem
{
    public class PostagemService : IPostagemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public PostagemService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<Post>> ObterPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> ObterPostPorIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post> CadastrarPostAsync(PostRequest postagem)
        {
            var userId = await _userService.GetUserIdAsync();

            var post = new Post
            {
                UsuarioId = userId,
                Titulo = postagem.Titulo,
                Descricao = postagem.Descricao,
                DataPostagem = DateTime.Now,
                Ativo = true
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task AtualizarPostAsync(int id, Post postagem)
        {
            if (id != postagem.Id)
            {
                throw new ArgumentException("ID inválido para atualização.");
            }

            _context.Posts.Update(postagem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverPostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Post não encontrado.");
            }
        }
    }
}