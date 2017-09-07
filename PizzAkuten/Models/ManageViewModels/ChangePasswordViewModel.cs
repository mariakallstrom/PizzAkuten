using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nuvarande lösenord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt lösenord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta nytt lösenord")]
        [Compare("NewPassword", ErrorMessage = "Lösenordet och det bekräftande lösenordet matchar inte!")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Statusmeddelande")]
        public string StatusMessage { get; set; }
    }
}
