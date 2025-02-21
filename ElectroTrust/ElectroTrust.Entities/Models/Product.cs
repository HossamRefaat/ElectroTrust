using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required, ForeignKey("Seller")]
        public int SellerID { get; set; }

        [Required, MaxLength(200)]
        public string ProductName { get; set; }

        [Required, ForeignKey("Category")]
        public int CategoryID { get; set; }

        public decimal BasePrice { get; set; }

        public string? Description { get; set; }

        public string? ImageURL { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Seller Seller { get; set; }
        public virtual Category Category { get; set; }

        // A product can have multiple reviews
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        // A product can be in multiple wishlists
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public virtual ICollection<Customization> Customizations { get; set; } = new List<Customization>();
    }
}
