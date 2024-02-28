using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ResponsibleId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Budget { get; set; }
        public DateTime InclusionAt { get; set; }
        public DateTime ReleaseYear { get; set; }
        public string Collors { get; set; }
        public SeasonEnum Season { get; set; }
        public StatusEnum Status { get; set; }
        public virtual User Responsible { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<Model> Models { get; set; }
    }
}
