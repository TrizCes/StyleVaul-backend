using StyleVaulAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StyleVaulAPI.Dto.Users.Request
{
    public class PutUsers
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Papel é obrigatório.")]
        public RoleEnum Role { get; set; }
    }
}
