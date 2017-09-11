using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.AccountViewModels
{
    public class RegisterViewModel
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

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        [StringLength(100, ErrorMessage = "Lösenordet måste vara minst 6 och max 100 tecken långt.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Lösenordet och det bekräftande lösenordet matchar inte!")]
        public string ConfirmPassword { get; set; }

        [Phone]
        [Required]
        [DisplayName("Telefonnummer")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefonnummer måste vara nummer")]
        public string Phone { get; set; }
    }
}
