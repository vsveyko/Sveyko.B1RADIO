using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sveyko.B1RADIO.Models
{
    public partial class Soundtrack
    {
        public int Id { get; set; }
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Genre is required")]
        public int GenreId { get; set; }
        [Display(Name = "Singer")]
        [Required(ErrorMessage = "Singer is required")]
        public int SingerId { get; set; }
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        [StringLength(150)]
        public string Title { get; set; }
        public string ServerFilename { get; set; }
        [Display(Name = "File name")]
        [Required(ErrorMessage = "File is required")]
        public string ClientFilename { get; set; }
        public Genre Genre { get; set; }
        public Singer Singer { get; set; }

        //[NotMapped]
        //public string CurrentSortOrder { get; set; }
        //[NotMapped]
        //public string CurrentFilter { get; set; }
        //[NotMapped]
        //public string CurrentSort { get; set; }
    }
}
