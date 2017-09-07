using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [DisplayName("Orderdatum")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Summa order")]
        public int TotalPrice { get; set; }
        [DisplayName("Levererad")]
        public bool Delivered { get; set; }
        public string ApplicationUserId { get; set; }
        [DisplayName("Användare")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("NonAccountUser")]
        public int NonAccountUserId { get; set; }
        [DisplayName("Användare utan konto")]
        public virtual NonAccountUser NonAccountUser { get; set; }
        [DisplayName("Kundvagn")]
        public Cart Cart { get; set; }
        [ForeignKey("Payment")]
        [DisplayName("BetalningsId")]
        public int PaymentId { get; set; }
        [DisplayName("Betalning")]
        public Payment Payment { get; set; }

    }
}
