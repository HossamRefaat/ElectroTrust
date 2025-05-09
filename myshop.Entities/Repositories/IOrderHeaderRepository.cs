﻿using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);

        void UpdateOrderStatus(int id, string orderStatus, string? PaymentStatus);

        ICollection<OrderHeader> GetOrderHeadersByUseId(string id);
    }
}
