using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required, ForeignKey("Order")]
        public int OrderID { get; set; }

        [ForeignKey("Customization")]
        public int? CustomizationID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Customization? Customization { get; set; }
    }

}
