using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        
        [Display(Name = "AnvändarNamn")]
        public string UserName { get; set; }
        [Display(Name = "Förnamn")]
        [Required]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "EfterNamn")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Gatunamn")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "Postnummer")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Postort")]
        public string City { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta Lösenord")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
