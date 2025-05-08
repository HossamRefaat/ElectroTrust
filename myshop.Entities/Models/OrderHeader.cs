using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        [Range(0.01, 1000000)]
        public decimal TotalPrice { get; set; }

        [StringLength(50)]
        public string? OrderStatus { get; set; }

        [StringLength(50)]
        public string? PaymentStatus { get; set; }

        [StringLength(100)]
        public string? TrackingNumber { get; set; }

        [StringLength(100)]
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(100)]
        public string? SessionId { get; set; }

        [StringLength(100)]
        public string? PaymentIntendId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
