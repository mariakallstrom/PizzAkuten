using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class NonAccountUser
    {
        public int NonAccountUserId { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string Street { get; set; }
        [Required]
        [MaxLength(10), MinLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ZipCode must be numeric")]
        public int ZipCode { get; set; }
        [Required]
        [MaxLength(100), MinLength(2)]
        public string City { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

    }
}
