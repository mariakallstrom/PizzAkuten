using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [MaxLength(100 ,ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Förnamn")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Efternamn")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Gata")]
        public string Street { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Postkoden får max ha 10 nummer"), MinLength(6,ErrorMessage = "Postkoden får minst ha 6 nummer")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postkoden måste vara nummer")]
        [DisplayName("Postkod")]
        public int ZipCode { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "{0} får max ha {2} tecken"), MinLength(2, ErrorMessage = "{0} får minst ha {2} tecken")]
        [DisplayName("Postort")]
        public string City { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [DisplayName("Telefonnummer")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefonnummer måste vara nummer")]
        public string Phone { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

    }
}
