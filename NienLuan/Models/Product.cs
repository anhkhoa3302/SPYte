using System;
using System.Collections.Generic;

namespace SPYte.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductCategory = new HashSet<ProductCategory>();
            ProductImgs = new HashSet<ProductImg>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public double Stock { get; set; }
        public string? Unit { get; set; }
        public int? IsVisible { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; } = null!;


        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
        public virtual ICollection<ProductImg> ProductImgs { get; set; }
    }
}
