namespace Blog.Web.Services.Autor
{
    public interface IAutorService
    {
        Task<IEnumerable<Blog.Data.Models.Autor>> GetAutoresAsync();
        Task<Blog.Data.Models.Autor> GetAutorByIdAsync(int id);
        Task AddAutorAsync(Blog.Data.Models.Autor autor);
        Task UpdateAutorAsync(Blog.Data.Models.Autor autor);
        Task DeleteAutorAsync(int id);
        bool AutorExists(int id);
    }
}