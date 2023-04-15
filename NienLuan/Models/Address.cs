using System;
using System.Collections.Generic;

namespace SPYte.Models
{
    public partial class Address
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string AddressDetail { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UserId { get; set; }
        public string? WardCode { get; set; }

        public virtual ApplicationUser? User { get; set; }
        
        public virtual Ward? WardCodeNavigation { get; set; }
    }
}
