using System.ComponentModel.DataAnnotations;

namespace LabaOne.Models
{
    public class VariantsEdit
    {
        public int Id { get; set; }
        [Display(Name = "Назва штаму")]
        public string? VariantName { get; set; }
        [Display(Name = "Походження штаму")]
        public string? VariantOrigin { get; set; }
        [Display(Name = "Дата відкриття штаму")]
        [DataType(DataType.Date)]
        public DateTime? VariantDateDiscovered { get; set; }
        [Display(Name = "Вірус")]
        public int? VirusId { get; set; }
        [Display(Name = "Список країн")]
        public List<int> CountriesIds { get; set; } = new List<int>();
        [Display(Name = "Список симптомів")]
        public List<int> SymptomsIds { get; set; } = new List<int>();

    }
}
