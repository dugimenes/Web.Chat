using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPosts() => Ok(_context.Posts.ToList());

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
        }
    }
}