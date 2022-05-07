using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabaOne
{
    public partial class VirusGroup
    {
        public VirusGroup()
        {
            Viruses = new List<Virus>();
        }
        public int Id { get; set; }
        [Display(Name = "Назва групи вірусів")]
        public string? GroupName { get; set; }
        [Display(Name = "Інформація про групу вірусів")]
        public string? GroupInfo { get; set; }
        public DateTime? DateDiscovered { get; set; }

        public virtual ICollection<Virus> Viruses { get; set; } = new List<Virus>();

    }
}
