using Volo.Abp.Application.Services;

namespace UTM.eCommerce.Services
{
    public interface ICrudAppService : IApplicationService
    {
        Task<Guid> CreateProductAsync(string name, decimal price, int stockCount);
        Task<Guid> PlaceOrderAsync(string customerFirstName, string customerLastName, string customerEmail, Guid productId, int quantity);
    }

}
