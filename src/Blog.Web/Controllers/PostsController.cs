using Blog.Data.Models;
using Blog.Web.Data;
using Blog.Web.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PostsController(UserManager<ApplicationUser> userManager, IPostService postService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _postService = postService;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPostsAsync(); return View(posts);
        }

        [Route("detalhes/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            
            return View(post);
        }

        [Route("novo")]
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome");
            return View();
        }

        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Titulo,Descricao,DataPostagem,DataAlteracaoPostagem,UsuarioId,Ativo")] Post post)
        {
            ModelState.Remove("Comentarios");

            if (ModelState.IsValid)
            {
                try
                {
                    await _postService.AddPostAsync(post);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao criar o post. Tente novamente mais tarde.");
                }
            }

            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", post.UsuarioId);
            return View(post);
        }

        [Route("editar/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            if (!await EhAdmin(post.UsuarioId))
            {
                return RedirectToAction("Permission", "Home");
            }
            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", post.UsuarioId); 
            
            return View(post);
        }

        [HttpPost("editar/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Titulo,Descricao,DataPostagem,DataAlteracaoPostagem,UsuarioId,Ativo")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Comentarios");

            if (ModelState.IsValid)
            {
                try
                {
                    await _postService.UpdatePostAsync(post);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_postService.PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewData["UsuarioId"] = new SelectList(_context.Autores, "Id", "Nome", post.UsuarioId); 
            return View(post);
        }

        [Route("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            if (!await EhAdmin(post.UsuarioId))
            {
                return RedirectToAction("Permission", "Home");
            }

            return View(post);
        }

        [HttpPost("excluir/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeletePostAsync(id); 
            
            return RedirectToAction(nameof(Index));
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

        public IActionResult Comments(int postId)
        {
            TempData["PostId"] = postId;

            return RedirectToAction("Create", "Comentarios");
        }
    }
}