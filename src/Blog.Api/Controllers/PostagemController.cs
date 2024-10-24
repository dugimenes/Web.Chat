using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/postagem")]
    public class PostagemController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PostagemController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Post>>> Obter()
        {
            return await _context.Posts.ToListAsync();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Post>> Obter(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            return post;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Post>> Cadastrar(Post postagem)
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Atualizar(int id, Post postagem)
        {
            if (id != postagem.Id) return BadRequest();

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.Posts.Update(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Remover(int id)
        {
            var postagem = await _context.Posts.FindAsync(id);

            _context.Posts.Remove(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}