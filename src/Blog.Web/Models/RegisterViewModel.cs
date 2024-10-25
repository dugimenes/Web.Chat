using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Sobrenome { get; set; }
    }
}
