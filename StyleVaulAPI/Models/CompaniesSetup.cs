using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Models
{
    public class CompanySetup
    {
        public int Id { get; set; }
        public ThemeEnum Theme { get; set; }
        public string Logo { get; set; }
        public int CompanySetupId { get; set; }
        public virtual Company Company { get; set; }
    }
}
