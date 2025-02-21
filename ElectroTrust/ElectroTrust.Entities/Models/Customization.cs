using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class Customization
    {
        [Key]
        public int CustomizationID { get; set; }

        [Required, ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [Required]
        public string CustomizationData { get; set; } // JSON string for color, text, overlay

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }

}
