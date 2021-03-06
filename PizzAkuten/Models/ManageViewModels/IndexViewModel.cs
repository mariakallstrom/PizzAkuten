﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
        
        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        
    }
}
