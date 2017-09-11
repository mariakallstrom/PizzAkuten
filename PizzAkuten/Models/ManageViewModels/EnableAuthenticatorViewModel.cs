using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.ManageViewModels
{
    public class EnableAuthenticatorViewModel
    {
            [Required]
            [StringLength(7, ErrorMessage = "Verifieringskoden måste vara minst 6 och max 100 tecken långt.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verifieringskod")]
            public string Code { get; set; }

            [ReadOnly(true)]
            public string SharedKey { get; set; }

            public string AuthenticatorUri { get; set; }
    }
}
