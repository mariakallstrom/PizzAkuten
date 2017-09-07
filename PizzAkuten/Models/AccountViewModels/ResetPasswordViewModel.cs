using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Lösenord")]
        [StringLength(100, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Lösenord", ErrorMessage = "Lösenordet och det bekräftande lösenordet matchar inte!")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
