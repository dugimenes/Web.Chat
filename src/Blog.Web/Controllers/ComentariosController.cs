﻿using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Blog.Web.Controllers
{
    [Authorize]
    [Route("comentarios")]
    public class ComentariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComentariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var usuarioId = user?.Id;

            var applicationDbContext = _context.Comentarios.Include(c => c.Autor).Include(c => c.Post);
            return _context.Comentarios != null ?
                            View(await applicationDbContext.Where(x => x.UsuarioId == usuarioId).ToListAsync()) :
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
                var user = await _userManager.GetUserAsync(User);
                comentario.UsuarioId = user?.Id;
                comentario.DataCadastro = DateTime.Now;

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

            if (!await EhAdmin(comentario.UsuarioId))
            {
                return Forbid("Não Permitido.");
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

            if (!await EhAdmin(comentario.UsuarioId))
            {
                return Forbid("Não Permitido.");
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

        [HttpGet("tem-permissao")]
        private async Task<bool> EhAdmin(int? ownerId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Admin") || ownerId == user.Id)
            {
                return true;
            }

            return false;
        }
    }
}
