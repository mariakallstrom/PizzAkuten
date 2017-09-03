using System;
using System.Collections.Generic;
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
        [MaxLength(100), MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string Street { get; set; }
        [Required]
        [MaxLength(10), MinLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ZipCode must be numeric")]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string City { get; set; }
       
    }
}
