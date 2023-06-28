using UTM.eCommerce.Entities;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Repositories
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
    }

}
