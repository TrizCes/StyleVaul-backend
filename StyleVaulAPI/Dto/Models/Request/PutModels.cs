using StyleVaulAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StyleVaulAPI.Dto.Models.Request
{
    public class PutModels
    {
        public int ResponsibleId { get; set; }
        public int CollectionId { get; set; }

        [Required(ErrorMessage = "O campo Nome do modelo é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Custo Real é obrigatório.")]
        public double RealCost { get; set; }

        [Required(ErrorMessage = "O campo Tipo do Modelo é obrigatório.")]
        [Range(
            1,
            9,
            ErrorMessage = "1 = Bermuda / 2 = Biquini / 3 = Bolsa / 4 = Boné / 5 = Calça / 6 = Calçados / 7 = Camisa / 8 = Chapéu / 9 = Saia"
        )]
        public ModelTypeEnum Type { get; set; }

        [Required(ErrorMessage = "O campo Bordado é obrigatório.")]
        public bool Embroidery { get; set; }

        [Required(ErrorMessage = "O campo Estampa é obrigatório.")]
        public bool Print { get; set; }
    }
}
