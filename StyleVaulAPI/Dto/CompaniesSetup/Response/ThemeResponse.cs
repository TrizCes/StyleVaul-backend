using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Dto.CompaniesSetup.Response
{
    public class ThemeResponse
    {
        public ThemeEnum Theme { get; set; }

        public ThemeResponse(ThemeEnum theme)
        {
            Theme = theme;
        }
    }
}
