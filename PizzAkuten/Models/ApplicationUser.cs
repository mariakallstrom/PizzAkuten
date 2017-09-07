﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzAkuten.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Förnamn")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Efternamn")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Gata")]
        public string Street { get; set; }
        [Required]
        [MaxLength(10), MinLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postkoden måste vara nummer")]
        [DisplayName("Postkod")]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Postort")]
        public string City { get; set; }
       
    }
}
