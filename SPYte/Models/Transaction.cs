using System;
using System.Collections.Generic;

namespace SPYte.Models
{
    public partial class Transaction
    {
        public long Id { get; set; }
        public long vnpayTranId { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public String vnp_SecureHash { get; set; }
        public String TerminalID { get; set; }
        public double vnp_Amount { get; set; }
        public string Unit { get; set; } = null!;
        public String bankCode { get; set; }
        public bool Status { get; set; }
        public string? UserId { get; set; }
        public long? OrderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual UserOrder? Order { get; set; } = null!;
        public virtual ApplicationUser? User { get; set; } = null!;
    }
}
