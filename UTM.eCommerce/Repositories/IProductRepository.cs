using UTM.eCommerce.Entities;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<Product> FindByNameAsync(string name);
    }

}
