using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPYte.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
        {
            Addresses = new HashSet<Address>();
            Products = new HashSet<Product>();
            Transactions = new HashSet<Transaction>();
            UserOrders = new HashSet<UserOrder>();
        }
        [PersonalData]
        [Column("firstName", TypeName = "nvarchar(10)")]
        public string? FirstName { get; set; }
        [PersonalData]
        [Column("middleName", TypeName = "nvarchar(10)")]
        public string? MiddleName { get; set; }
        [PersonalData]
        [Column("lastName", TypeName = "nvarchar(15)")]
        public string? LastName { get; set; }
        [PersonalData]
        [Column("avatarPicUrl", TypeName = "nvarchar(200)")]
        public string? avatarPicUrl { get; set; }
        [PersonalData]
        [Column("userDescription")]
        public string? userDescription { get; set; }
        [PersonalData]
        [Column("status")]
        public int? Status { get; set; }
        [PersonalData]
        [Column("createdDate")]
        public DateTime? CreatedDate { get; set; }
        [PersonalData]
        [Column("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [PersonalData]
        [Column("bannedDate")]
        public DateTime? BannedDate { get; set; }
        [PersonalData]
        [Column("bannedTime")]
        public DateTime? BannedTime { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserOrder> UserOrders { get; set; }
    }
}
