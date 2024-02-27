using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Dto.Users.Response
{
    public class UsersResponse
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public RoleEnum RoleEnum { get; set; }
    }
}
