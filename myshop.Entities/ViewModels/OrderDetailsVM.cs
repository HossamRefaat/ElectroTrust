using myshop.Entities.ViewModels;
using System;
using System.Collections.Generic;

namespace myshop.ViewModels
{
    public class OrderDetailViewModel
    {
        // Header Information
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }

        // List of items in the order
        public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>(); // Initialize
    }
}