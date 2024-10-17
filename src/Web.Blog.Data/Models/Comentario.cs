namespace Web.Blog.Data.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public int UsuarioId { get; set; }
        public virtual Autor Usuario { get; set; }
        public bool Ativo { get; set; }
    }
}
