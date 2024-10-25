using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato incorreto")]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }
        
        public int PostId { get; set; }
        
        public virtual Post Post { get; set; }
        
        public int? UsuarioId { get; set; }
        
        public virtual Autor Autor { get; set; }
        
        public bool Ativo { get; set; }
    }
}