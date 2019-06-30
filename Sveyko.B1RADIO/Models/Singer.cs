using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sveyko.B1RADIO.Models
{
    public partial class Singer
    {
        public Singer()
        {
            Soundtrack = new HashSet<Soundtrack>();
        }

        public int Id { get; set; }
        [Display(Name = "Singer")]
        [Required(ErrorMessage = "Singer is required")]
        [StringLength(150)]
        public string Name { get; set; }

        public ICollection<Soundtrack> Soundtrack { get; set; }
    }
}
