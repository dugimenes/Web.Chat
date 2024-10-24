using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Models
{
    public class Autor
    {
        [Key, ForeignKey("User")]
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
        
        public ApplicationUser User { get; set; }

    }
}