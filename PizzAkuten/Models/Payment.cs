using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Payment
    {

        public int PaymentId { get; set; }
      
        public string PayMethod { get; set; }
        public string CardNumber { get; set; }
   
        public int Cvv { get; set; }
    
        public int Year { get; set; }
 
        public int Month { get; set; }
       
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public bool IsPaid { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationuserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("NonAccountUser")]
        public int NonAccountUserId { get; set; }
        public NonAccountUser NonAccountUser { get; set; }



    }
}
