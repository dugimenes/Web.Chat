using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/postagem")]
    public class PostsController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        readonly JwtSettings _jwtSettings;
        

        public PostsController(SignInManager<ApplicationUser> signInManager, 
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
        public async Task<ActionResult<IEnumerable<Post>>> Obter()
        {
            return await _context.Posts.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> Obter(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            return post;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CadastrarPostagem(Post postagem)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Erros de validação"
                });
            }

            _context.Posts.Add(postagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Obter), new { id = postagem.Id }, postagem);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> AtualizarPostagem(int id, Post postagem)
        {
            if (id != postagem.Id) return BadRequest();

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.Posts.Update(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePostagem(int id)
        {
            var postagem = await _context.Posts.FindAsync(id);

            _context.Posts.Remove(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}