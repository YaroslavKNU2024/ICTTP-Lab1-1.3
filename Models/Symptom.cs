using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabaOne
{
    public partial class Symptom
    {
        public Symptom() { }

        public int Id { get; set; }
        [Display(Name = "Симптом")]
        public string? SymptomName { get; set; }

        //public virtual ICollection<SymptomsVariant> SymptomsVariants { get; set; } = new HashSet<SymptomsVariant>();
        [Display(Name = "Штам")]
        public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();

    }
}
