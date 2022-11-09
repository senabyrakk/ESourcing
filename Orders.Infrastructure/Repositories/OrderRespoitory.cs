using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Repositories
{
    public class OrderRespoitory : BaseRepository<Order>, IOrderRepository
    {
        public OrderRespoitory(OrderContext orderContext) : base(orderContext)
        {

        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName)
        {
            var orderList = await _dbContext.Order
                                     .Where(o => o.SellerUserName == userName)
                                     .ToListAsync();

            return orderList;
        }
    }
}
