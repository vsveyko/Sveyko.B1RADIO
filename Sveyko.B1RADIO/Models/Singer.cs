using System;
using System.Collections.Generic;

namespace Sveyko.B1RADIO.Models
{
    public partial class Singer
    {
        public Singer()
        {
            Soundtrack = new HashSet<Soundtrack>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Soundtrack> Soundtrack { get; set; }
    }
}
