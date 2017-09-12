using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Payment
    {

        public int PaymentId { get; set; }
      
        [Required]
        [DisplayName("Betalningsmetod")]
        public string PayMethod { get; set; }
        [MinLength(16, ErrorMessage = "Kortnummer måste var 16 nummer"), MaxLength(16, ErrorMessage = "Kortnummer måste vara 16 nummer")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Kortnummer måste vara siffror")]
        [DisplayName("Kortnummer")]
        public string CardNumber { get; set; }

        [Range(100,999)]
        public int Cvv { get; set; }
        [DisplayName("År")]
        public int Year { get; set; }
        [DisplayName("Månad")]
        public int Month { get; set; }
        [DisplayName("Betalad")]
        public bool IsPaid { get; set; }
 
 



    }
}
