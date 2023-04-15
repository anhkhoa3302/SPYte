using System;
using System.Collections.Generic;

namespace SPYte.Models
{
    public partial class ProductImg
    {
        public long Id { get; set; }
        public string Url { get; set; } = null!;
        public int? IsCover { get; set; }
        public long ProductId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
