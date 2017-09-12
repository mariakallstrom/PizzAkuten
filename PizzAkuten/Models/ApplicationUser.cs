using System;
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
        [MaxLength(100, ErrorMessage = "Förnamn får max ha 100 tecken"), MinLength(2, ErrorMessage = "Förnamn får minst ha 2 tecken")]
        [DisplayName("Förnamn")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Efternamn får max ha 100 tecken"), MinLength(2, ErrorMessage = "Efternamn får minst ha 2 tecken")]
        [DisplayName("Efternamn")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Postadress får max ha 100 tecken"), MinLength(2, ErrorMessage = "Postadress får minst ha 2 tecken")]
        [DisplayName("Postadress")]
        public string Street { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Postkoden får max ha 10 nummer"), MinLength(5, ErrorMessage = "Postkoden får minst ha 5 nummer")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postkoden måste vara nummer")]
        [DisplayName("Postkod")]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Postort får max ha 100 tecken"), MinLength(2, ErrorMessage = "Postort får minst ha 2 tecken")]
        [DisplayName("Postort")]
        public string City { get; set; }
      
    }
}
