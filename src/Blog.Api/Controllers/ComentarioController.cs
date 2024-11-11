using Blog.Api.Request;
using Blog.Api.Services.Comentario;
using Blog.Data.Models;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;
        private readonly IUserService _userService;

        public ComentarioController(IComentarioService comentarioService, IUserService userService)
        {
            _comentarioService = comentarioService;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Comentario>>> Obter()
        {
            var comentarios = await _comentarioService.ObterComentariosAsync();
            return Ok(comentarios);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Comentario>> Obter(int id)
        {
            var comentario = await _comentarioService.ObterComentarioPorIdAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Comentario>> Cadastrar(ComentarioRequest comentarioRequest)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Erros de validação"
                });
            }

            var userId = await _userService.GetUserIdAsync();
            var comentario = await _comentarioService.CadastrarComentarioAsync(comentarioRequest, userId);

            return CreatedAtAction(nameof(Obter), new { id = comentario.Id }, comentario);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Atualizar(int id, Comentario comentario)
        {
            if (id != comentario.Id) return BadRequest();

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            await _comentarioService.AtualizarComentarioAsync(id, comentario);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Remover(int id)
        {
            await _comentarioService.RemoverComentarioAsync(id);

            return NoContent();
        }
    }
}