using System.Diagnostics;
using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using UTM.eCommerce.Services.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Services
{
    public class ProductAppService : ApplicationService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductAppService(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<Guid> CreateProduct(string name, int price, int stock)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

            var created = await _productRepository.InsertAsync(product);
            
            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

            return created.Id;
        }

        public async Task<bool> ProductExists(string name)
        {
            var existingProduct = await _productRepository.FirstOrDefaultAsync(p => p.Name == name);
            return existingProduct != null;
        }
    }

}
