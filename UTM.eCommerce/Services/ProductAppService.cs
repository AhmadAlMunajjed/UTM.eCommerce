using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using UTM.eCommerce.Services.Dtos;
using Volo.Abp.Application.Services;

namespace UTM.eCommerce.Services
{
    public class ProductAppService : ApplicationService
    {
        private readonly IProductRepository _productRepository;

        public ProductAppService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }


        public async Task<Guid> CreateProductAsync(string name, decimal price, int stockCount)
        {
            var existingProduct = await _productRepository.FindByNameAsync(name);
            if (existingProduct != null)
            {
                throw new ApplicationException("Product with the same name already exists.");
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                StockCount = stockCount
            };

            await _productRepository.InsertAsync(product);

            return product.Id;
        }
    }

}
