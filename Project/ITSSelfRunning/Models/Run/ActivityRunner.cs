using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSSelfRunning.Models.Run
{
    public class ActivityRunner
    {
        public int IdActivity { get; set; }
        public int IdRunner { get; set; }
        public int Type { get; set; }
        public string UriGara { get; set; }
    }
}
