using System.Diagnostics;
using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Services
{
    public class CrudAppService : ApplicationService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CrudAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<Order, Guid> orderRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async void CreateProduct(string name, decimal price, int stockCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Code Smell: Duplicated Code
            var existingProduct = await _productRepository.FirstOrDefaultAsync(p => p.Name == name);
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

            var customer = await GetOrCreateCustomerAsync();

            await CreateOrder(product, customer);

            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
        }

        public async Task<bool> CheckIfProductExists(string name)
        {
            // Code Smell: Duplicated Code
            var existingProduct = await _productRepository.FirstOrDefaultAsync(p => p.Name == name);
            return existingProduct != null;
        }

        private async Task<Customer> GetOrCreateCustomerAsync()
        {
            // Design Smell: Lack of Abstraction
            var customers = await _customerRepository.GetListAsync();
            if (customers.Count == 0)
            {
                throw new ApplicationException("No customers found.");
            }

            var customer = customers.FirstOrDefault(c => c.FirstName == "John" && c.LastName == "Doe");

            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com"
                };
                await _customerRepository.InsertAsync(customer);
            }

            return customer;
        }

        private async Task CreateOrder(Product product, Customer customer)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = product.Price,
                Customer = customer
            };
            customer.Orders.Add(order);

            await _orderRepository.InsertAsync(order);
        }


    }
}
