using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITSSelfRunning.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Sex { get; set; }
        public DateTime Birthday { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoUri { get; set; }
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
