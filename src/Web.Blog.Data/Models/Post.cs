namespace Web.Blog.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataPostagem { get; set; }
        public DateTime? DataAlteracaoPostagem { get; set; }
        public int UsuarioId { get; set; }
        public virtual Autor Usuario { get; set; }
        public bool Ativo { get; set; }
    }
}