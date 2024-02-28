using System.ComponentModel.DataAnnotations;

namespace StyleVaulAPI.Dto.Companies.Request
{
    public class PostCompanies
    {
        [Required(ErrorMessage = "O campo Nome da empresa é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo CNPJ é obrigatório.")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "O campo Nome do gerente é obrigatório.")]
        public string Manager { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Confirmação de Senha é obrigatório.")]
        public string PasswordConfirmation { get; set; }
    }
}
