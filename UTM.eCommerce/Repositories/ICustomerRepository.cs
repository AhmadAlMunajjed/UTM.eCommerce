using UTM.eCommerce.Entities;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> FindByEmailAsync(string email);
    }
}
