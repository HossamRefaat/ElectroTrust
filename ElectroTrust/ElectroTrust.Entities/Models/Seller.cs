using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class Seller
    {
        [Key, ForeignKey("User")]
        public int SellerID { get; set; }

        [Required, MaxLength(200)]
        public string StoreName { get; set; }

        public string? StoreDescription { get; set; }

        // Navigation property
        public virtual User User { get; set; }

        // A seller can have multiple products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
