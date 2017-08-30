using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalPrice { get; set; }
        public bool Delivered { get; set; }
        public string ApplicationuserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("NonAccountUser")]
        public int NonAccountUserId { get; set; }
        public virtual NonAccountUser NonAccountUser { get; set; }
        public virtual OrderDish OrderDish { get; set; }
        public Payment Payment { get; set; }

    }
}
