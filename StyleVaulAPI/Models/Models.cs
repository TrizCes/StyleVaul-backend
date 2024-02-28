using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Models
{
    public class Model
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ResponsibleId { get; set; }
        public int CollectionId { get; set; }
        public string Name { get; set; }
        public DateTime InclusionAt { get; set; }
        public double RealCost { get; set; }
        public ModelTypeEnum Type { get; set; }
        public bool Embroidery { get; set; }
        public bool Print { get; set; }

        public virtual User Responsible { get; set; }
        public virtual Collection Collection { get; set; }
        public virtual Company Company { get; set; }
    }
}
