using System;
using System.Collections.Generic;

namespace LabaOne
{
    public partial class CountriesVariant
    {
        public int Id { get; set; }

        public int CountryId { get; set; }
        public int VariantId { get; set; }
        //public int? Cases { get; set; }
        //public int? Dead { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual Variant Variant { get; set; } = null!;
    }
}
