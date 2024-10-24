using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/comentario")]
    public class ComentarioController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        readonly JwtSettings _jwtSettings;

        public ComentarioController(SignInManager<ApplicationUser> signInManager, 
                                    UserManager<ApplicationUser> userManager, 
                                    IOptions<JwtSettings> jwtSettings, 
                                    ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> Obter()
        {
            return await _context.Comentarios.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> Obter(int id)
        {
            var comentario = await _context.Posts.FindAsync(id);

            return comentario;
        }

        [HttpPost]
        public async Task<ActionResult<Comentario>> CadastrarComentario(Comentario comentario)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Erros de validação"
                });
            }

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Obter), new { id = comentario.Id }, comentario);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> AtualizarComentario(int id, Comentario comentario)
        {
            if (id != comentario.Id) return BadRequest();

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}