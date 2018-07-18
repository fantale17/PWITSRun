using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdRunner { get; set; }
        public DateTime CreationDate { get; set; }
        public string Place { get; set; }
        public int Type { get; set; }
        public string UriGara { get; set; }
        public int Status;

    }
}

