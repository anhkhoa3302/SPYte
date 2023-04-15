using System;
using System.Collections.Generic;

namespace SPYte.Models
{
    public partial class OrderDetail
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public long? ProductId { get; set; }
        public long? OrderId { get; set; }

        public virtual UserOrder? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
