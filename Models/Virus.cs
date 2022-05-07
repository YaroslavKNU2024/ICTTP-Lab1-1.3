using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabaOne
{
    public partial class Virus
    {
        public Virus()
        {
            Variants = new List<Variant>();
        }

        public int Id { get; set; }

        [Display(Name = "Назва вірусу")]
        public string? VirusName { get; set; }
        [Display(Name = "Дата відкриття вірусу")]
        [DataType(DataType.Date)]
        public DateTime? VirusDateDiscovered { get; set; }

        // Для фронту
        [Display(Name = "Назва групи вірусів")]
        public int? GroupId { get; set; }

        [Display(Name = "Група")]
        public virtual VirusGroup? Group { get; set; }
        public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();

    }
}
