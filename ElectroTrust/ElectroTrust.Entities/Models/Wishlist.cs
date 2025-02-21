using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistID { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [Required, ForeignKey("Product")]
        public int ProductID { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }

}
