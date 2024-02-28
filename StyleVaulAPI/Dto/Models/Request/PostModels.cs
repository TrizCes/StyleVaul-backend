using StyleVaulAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StyleVaulAPI.Dto.Models.Request
{
    public class PostModels
    {
        [Required(ErrorMessage = "O campo Nome do Modelo é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Nome do Responsável é obrigatório.")]
        public int ResponsibleId { get; set; }

        [Required(ErrorMessage = "O campo Coleção relacionada é obrigatório.")]
        public int CollectionId { get; set; }

        [Required(ErrorMessage = "O campo Custo Real é obrigatório.")]
        public double RealCost { get; set; }

        [Required(ErrorMessage = "O campo Tipo do Modelo é obrigatório.")]
        public ModelTypeEnum Type { get; set; }

        [Required(ErrorMessage = "O campo Bordado é obrigatório.")]
        public bool Embroidery { get; set; }

        [Required(ErrorMessage = "O campo Estampa é obrigatório.")]
        public bool Print { get; set; }
    }
}
