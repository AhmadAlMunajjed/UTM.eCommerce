using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using UTM.eCommerce.Services.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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

        public async void CreateProduct(string name, int price, int stock)
        {
            if (await ProductExists(name))
            {
                throw new ApplicationException("Product with the same name already exists.");
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                StockCount = stock
            };

            await _productRepository.InsertAsync(product);
        }

        public async Task<bool> ProductExists(string name)
        {
            var existingProduct = await _productRepository.FirstOrDefaultAsync(p => p.Name == name);
            return existingProduct != null;
        }
    }

}
