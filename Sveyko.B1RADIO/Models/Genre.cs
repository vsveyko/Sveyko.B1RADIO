using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sveyko.B1RADIO.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Soundtrack = new HashSet<Soundtrack>();
        }

        public int Id { get; set; }
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Genre is required")]
        [StringLength(150)]
        public string Name { get; set; }

        public ICollection<Soundtrack> Soundtrack { get; set; }
    }
}
