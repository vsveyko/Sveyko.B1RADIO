using System;
using System.Collections.Generic;

namespace Sveyko.B1RADIO.Models
{
    public partial class Soundtrack
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int SingerId { get; set; }
        public string Title { get; set; }
        public string Filepath { get; set; }

        public Genre Genre { get; set; }
        public Singer Singer { get; set; }
    }
}
