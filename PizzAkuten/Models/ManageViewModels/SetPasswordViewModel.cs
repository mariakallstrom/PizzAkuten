using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.ManageViewModels
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Lösenordet måste vara minst 6 och max 100 tecken långt..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt lösenord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta nytt lösenord")]
        [Compare("Nytt lösenord", ErrorMessage = "Lösenordet och det bekräftande lösenordet matchar inte!")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
