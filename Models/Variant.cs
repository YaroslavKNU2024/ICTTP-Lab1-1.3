using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabaOne
{
    public partial class Variant
    {
        public Variant() { }

        public int Id { get; set; }
        [Display(Name = "Назва штаму вірусу")]
        public string? VariantName { get; set; }
        [Display(Name = "Походження штаму")]
        public string? VariantOrigin { get; set; }
        [Display(Name = "Дата відкриття штаму")]
        [DataType(DataType.Date)]
        public DateTime? VariantDateDiscovered { get; set; }
        [Display(Name = "Id вірусів")]
        public int? VirusId { get; set; }

        public virtual Virus? Virus { get; set; }
        [Display(Name = "Список країн")]
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
        [Display(Name = "Список симптомів")]
        public virtual ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
    }
}
