using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    class MyOrdersVM
    {
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
