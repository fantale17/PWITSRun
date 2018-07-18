using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITSSelfRunning.Models.RunViewModels
{
    public class ActivityViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Place { get; set; }
    }
}
