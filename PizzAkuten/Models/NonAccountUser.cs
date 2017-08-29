using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class NonAccountUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Street { get; set; }

        public int ZipCode { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }

        public Payment Payment { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
