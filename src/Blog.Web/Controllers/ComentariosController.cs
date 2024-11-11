using Blog.Data.Models;
using Blog.Web.Data;
using Blog.Web.Services.Comentario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize]
    [Route("comentarios")]
    public class ComentariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IComentarioService _comentarioService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComentariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IComentarioService comentarioService)
        {
            _context = context;
            _userManager = userManager;
            _comentarioService = comentarioService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); 
            var comentarios = await _comentarioService.GetComentariosAsync(user?.Id); 
            
            return View(comentarios);
        }

        [Route("detalhes/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var comentario = await _comentarioService.GetComentarioByIdAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        [Route("novo")]
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Descricao");
            
            return View();
        }
        
        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,DataCadastro,PostId,UsuarioId,Ativo")] Comentario comentario)
        {
            if (TempData.ContainsKey("PostId"))
            {
                comentario.PostId = (int)TempData["PostId"];
            }

            ModelState.Remove("Id"); 
            ModelState.Remove("AutorId"); 
            ModelState.Remove("PostId");

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    await _comentarioService.AddComentarioAsync(comentario, user);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao criar o post. Tente novamente mais tarde.");
                }
            }

            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", comentario.UsuarioId); 
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Descricao", comentario.PostId); 
            
            return View(comentario);
        }

        [Route("editar/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var comentario = await _comentarioService.GetComentarioByIdAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            if (!await _comentarioService.EhAdminAsync(user, comentario.UsuarioId))
            {
                return RedirectToAction("Permission", "Home");
            }

            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", comentario.UsuarioId); 
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Descricao", comentario.PostId); 
            
            return View(comentario);
        }

        [HttpPost("editar/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,DataCadastro,PostId,UsuarioId,Ativo")] Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _comentarioService.UpdateComentarioAsync(comentario);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_comentarioService.ComentarioExists(comentario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", comentario.UsuarioId); ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Descricao", comentario.PostId); return View(comentario);
        }

        [Route("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comentario = await _comentarioService.GetComentarioByIdAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (!await _comentarioService.EhAdminAsync(user, comentario.UsuarioId))
            {
                return RedirectToAction("Permission", "Home");
            }

            return View(comentario);
        }

        [HttpPost("excluir/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _comentarioService.DeleteComentarioAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}