using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Dto.Models.Response
{
    public class ModelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResponsibleId { get; set; }
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public double RealCost { get; set; }
        public ModelTypeEnum Type { get; set; }
        public bool Embroidery { get; set; }
        public bool Print { get; set; }
    }
}
