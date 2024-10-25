using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Authorization;
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

        public ComentariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comentarios.Include(c => c.Autor).Include(c => c.Post);
            return _context.Comentarios != null ?
                            View(await applicationDbContext.ToListAsync()) :
                            Problem("Entity set 'ApplicationDbContext.Comentarios' is null.");
        }

        [Route("detalhes/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            if (_context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.Autor)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);

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
            ModelState.Remove("Id");
            ModelState.Remove("AutorId");

            if (ModelState.IsValid)
            {
                _context.Add(comentario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", comentario.UsuarioId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Descricao", comentario.PostId);
            return View(comentario);
        }

        [Route("editar/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
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
                    _context.Update(comentario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentarioExists(comentario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", comentario.UsuarioId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Descricao", comentario.PostId);
            return View(comentario);
        }

        [Route("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.Autor)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        [HttpPost("excluir/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("existe-comentario")]
        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.Id == id);
        }
    }
}
