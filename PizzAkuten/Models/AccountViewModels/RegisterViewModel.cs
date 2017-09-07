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
        
        [Display(Name = "Email")]
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
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        [StringLength(100, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Lösenord", ErrorMessage = "Lösenordet och det bekräftande lösenordet matchar inte!")]
        public string ConfirmPassword { get; set; }
    }
}
