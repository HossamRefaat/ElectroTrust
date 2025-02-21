using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class Customer
    {
        [Key, ForeignKey("User")]
        public int CustomerID { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        // Navigation property
        public virtual User User { get; set; }

        // A customer can have multiple orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // A customer can create multiple reviews
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        // A customer can have multiple items in their wishlist
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public virtual ICollection<Customization> Customizations { get; set; } = new List<Customization>();
    }
}
