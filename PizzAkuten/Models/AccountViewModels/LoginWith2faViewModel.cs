using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "Autentiseringskoden måste vara minst 6 och max 100 tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Autentiseringskod")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Kom ihåg denna dator")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
