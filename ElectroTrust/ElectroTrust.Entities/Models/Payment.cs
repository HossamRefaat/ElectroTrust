using ElectroTrust.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.Entities.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required, ForeignKey("Order")]
        public int OrderID { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; } // Enum: PayPal, Stripe, COD

        public string? TransactionID { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending; // Enum

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Order Order { get; set; }
    }
}
