﻿using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<OrderHeader?> GetOrderHeadersByUseId(string id)
        {
            var orderHeaders = _context.OrderHeaders
                .Include(x => x.OrderDetails)
                .Where(x => x.ApplicationUserId == id)
                .ToList();

            return orderHeaders;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }
          
        public void UpdateOrderStatus(int id, string orderStatus, string? PaymentStatus)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null) 
            {
                orderFromDb.OrderStatus = orderStatus;
                orderFromDb.PaymentDate = DateTime.Now;
                if(PaymentStatus != null)
                {
                    orderFromDb.PaymentStatus = PaymentStatus;
                }
            }
        }
    }
}
