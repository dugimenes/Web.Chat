namespace Blog.Data.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Admin { get; set; }
    }
}