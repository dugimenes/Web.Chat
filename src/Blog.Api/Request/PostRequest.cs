using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Request
{
    public class PostRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(40000, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} está em formato incorreto")]
        [Display(Name = "Data de Postagem")]
        public DateTime DataPostagem { get; set; }
    }
}
