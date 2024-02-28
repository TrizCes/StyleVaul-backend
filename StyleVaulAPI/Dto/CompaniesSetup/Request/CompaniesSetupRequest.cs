using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Dto.CompaniesSetup.Request
{
    public class CompaniesSetupRequest
    {
        public ThemeEnum Theme { get; set; }
        public string Logo { get; set; }
    }
}
