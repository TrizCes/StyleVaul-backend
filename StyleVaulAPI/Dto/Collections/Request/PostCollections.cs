using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using StyleVaulAPI.Models.Enums;
namespace StyleVaulAPI.Dto.Collections.Request
{
    public class PostCollectionsDto
    {
        [JsonIgnore]
        public int ResponsibleId { get; set; }

        [Required(ErrorMessage = "O nome de coleção é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo marca é obrigatório.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "O campo de orçamento é obrigatório.")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "O campo Ano de Lançamento é obrigatório.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Range(
            typeof(DateTime),
            "01/01/1900",
            "31/12/2050",
            ErrorMessage = "A Data de Lançamento deve estar entre {1} e {2}."
        )]
        public DateTime ReleaseYear { get; set; }

        public string Collors { get; set; }

        [Required(ErrorMessage = "O campo estação é obrigatório.")]
        [Range(1, 4, ErrorMessage = "1 = Autumn / 2 = Winter / 3 = Spring / 4 = Summer")]
        public SeasonEnum Season { get; set; }

        [Required(ErrorMessage = "O campo de status é obrigatório.")]
        [Range(
            1,
            4,
            ErrorMessage = "1 = NotStarted / 2 = InProgress / 3 = Completed / 4 = Archived "
        )]
        public StatusEnum Status { get; set; }
    }
}
