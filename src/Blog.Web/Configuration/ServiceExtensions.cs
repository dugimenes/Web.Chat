using Blog.Web.Services.Autor;
using Blog.Web.Services.Comentario;
using Blog.Web.Services.Post;

namespace Blog.Web.Configuration
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IComentarioService, ComentarioService>();
            services.AddScoped<IAutorService, AutorService>();
        }
    }
}