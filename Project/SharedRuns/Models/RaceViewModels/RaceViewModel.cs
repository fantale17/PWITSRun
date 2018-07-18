using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedRuns.Models.RaceViewModels
{
    public class RaceViewModel
    {
        public string Name { get; set; }
        public string IdOrganizer { get; set; }
        public string Place { get; set; }
        public DateTime CreationDate { get; set; }
        public string UriGara { get; set; }
    }
}
