using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public class OrderItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal ItemTotal => Quantity * PricePerItem; // Calculated total for the line item
        public string? ImageUrl { get; set; } // Holds the URL for the product image
    }
}
