using Blog.Api.Request;
using Blog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services.Comentario
{
    public class ComentarioService : IComentarioService
    {
        private readonly ApplicationDbContext _context;

        public ComentarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Data.Models.Comentario>> ObterComentariosAsync()
        {
            return await _context.Comentarios.ToListAsync();
        }

        public async Task<Data.Models.Comentario> ObterComentarioPorIdAsync(int id)
        {
            return await _context.Comentarios.FindAsync(id);
        }

        public async Task<Data.Models.Comentario> CadastrarComentarioAsync(ComentarioRequest comentarioRequest, int? userId)
        {
            var comentario = new Data.Models.Comentario
            {
                UsuarioId = userId,
                PostId = comentarioRequest.PostId,
                Descricao = comentarioRequest.Descricao,
                DataCadastro = DateTime.Now,
                Ativo = true
            };

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return comentario;
        }

        public async Task AtualizarComentarioAsync(int id, Data.Models.Comentario comentario)
        {
            if (id != comentario.Id)
            {
                throw new ArgumentException("ID inválido para atualização.");
            }

            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverComentarioAsync(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Comentário não encontrado.");
            }
        }
    }
}