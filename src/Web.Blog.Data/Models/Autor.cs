using Microsoft.AspNetCore.Identity;

namespace Web.Blog.Data.Models
{
    public class Autor : IdentityUser<int>
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Admin { get; set; }
    }
}