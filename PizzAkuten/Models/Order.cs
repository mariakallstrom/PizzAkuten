using System;
using System.Collections.Generic;
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
        public virtual OrderDish OrderDish { get; set; }

    }
}
