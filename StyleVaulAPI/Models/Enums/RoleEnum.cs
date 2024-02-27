using System.ComponentModel;

namespace StyleVaulAPI.Models.Enums
{
    public enum RoleEnum
    {
        [Description("Gerente")]
        Manager = 1,

        [Description("Somente ver")]
        ReadOnly = 2,

        [Description("Time")]
        Team = 3,

        [Description("Admin")]
        Admin = 4
    }
}
