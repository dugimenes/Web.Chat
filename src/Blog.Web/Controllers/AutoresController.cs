using Blog.Data.Models;
using Blog.Web.Data;
using Blog.Web.Services.Autor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize]
    [Route("autores")]
    public class AutoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAutorService _autorService;

        public AutoresController(ApplicationDbContext context, IAutorService autorService)
        {
            _context = context;
            _autorService = autorService;
        }

        
        public async Task<IActionResult> Index()
        {
            var autores = await _autorService.GetAutoresAsync(); 
            
            return View(autores);
        }

        [Route("detalhes/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var autor = await _autorService.GetAutorByIdAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [Route("novo")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,SobreNome,DataCadastro")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _autorService.AddAutorAsync(autor);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao criar o post. Tente novamente mais tarde.");
                }
            }

            return View(autor);
        }

        [Route("editar/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var autor = await _autorService.GetAutorByIdAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost("editar/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,SobreNome,DataCadastro")] Autor autor)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _autorService.UpdateAutorAsync(autor);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_autorService.AutorExists(autor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(autor);
        }

        [Route("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var autor = await _autorService.GetAutorByIdAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost("excluir/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _autorService.DeleteAutorAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _autorService.AutorExists(id);
        }
    }
}