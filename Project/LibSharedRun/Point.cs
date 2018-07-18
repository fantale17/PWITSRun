using System;
using System.Collections.Generic;
using System.Text;

namespace LibSharedRun
{
    public class Point
    {
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public DateTime Time { get; set; }
        public int IdActivity { get; set; }
        public string UriSelfie { get; set; }
        public int IdRunner { get; set; }
    }
}
