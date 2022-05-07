using System;
using System.Collections.Generic;

namespace LabaOne
{
    public partial class SymptomsVariant
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public int SymptomId { get; set; }
        //public int? Cases { get; set; }

        public virtual Symptom Symptom { get; set; } = null!;
        public virtual Variant Variant { get; set; } = null!;
    }
}
