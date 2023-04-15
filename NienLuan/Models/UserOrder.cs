using System;
using System.Collections.Generic;

namespace SPYte.Models
{
    public partial class UserOrder
    {
        public UserOrder()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Transactions = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public double Total { get; set; }
        public double GrandTotal { get; set; }
        public string CustomerName { get; set; } = null!;
        public string PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public string AddressId { get; set; }
        public int? Status { get; set; }
        public string? Shipping { get; set; }
        public string? ShippingVia { get; set; }
        public string? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
