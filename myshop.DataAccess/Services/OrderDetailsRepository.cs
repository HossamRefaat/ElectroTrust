using Microsoft.EntityFrameworkCore;
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
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<OrderDetails> GetAllByOrderHeaderId(int orderId)
        {
            var orderDetails = _context.OrderDetails
                .Include(o => o.Product)
                .Where(o => o.OrderHeaderId == orderId)
                .ToList();

            return orderDetails;
        }

        public void Update(OrderDetails orderDetails)
        {
            _context.OrderDetails.Update(orderDetails);
        }

       
    }
}
