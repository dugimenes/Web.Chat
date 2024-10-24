using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} tem que ter entre {1} e {2} caracteres")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} tem que ter entre {1} e {2} caracteres")]
        public string? SobreNome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato incorreto")]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        public bool Admin { get; set; }
    }
}