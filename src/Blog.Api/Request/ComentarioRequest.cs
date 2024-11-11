using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Request
{
    public class ComentarioRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        public string? Descricao { get; set; }
    }
}