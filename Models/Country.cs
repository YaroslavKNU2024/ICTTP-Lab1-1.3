using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabaOne
{
    public partial class Country
    {
        public Country() { }

        public int Id { get; set; }
        [Display(Name = "Назва країни")]
        public string? CountryName { get; set; }
        [Display(Name = "Штами")]
        public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();
    }
}
