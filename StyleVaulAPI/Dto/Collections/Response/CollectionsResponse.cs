using StyleVaul.Models.Enums;

namespace StyleVaulAPI.Dto.Collections.Response
{
    public class CollectionsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ResponsibleId { get; set; }
        public string ResponsibleName { get; set; }
        public string Brand { get; set; }
        public decimal Budget { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime InclusionAt { get; set; }
        public DateTime ReleaseYear { get; set; }
        public string Collors { get; set; }
        public SeasonEnum Season { get; set; }
        public StatusEnum Status { get; set; }
    }
}
