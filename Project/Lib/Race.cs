using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public class Race
    {
        public string Name { get; set; }
        public string IdOrganizer { get; set; }
        public string Place { get; set; }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int Type { get; set; }
        public string UriGara { get; set; }
    }
}
