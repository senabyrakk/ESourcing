using Orders.Domain.Entities;
using Orders.Domain.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.Domain.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName);
    }
}
