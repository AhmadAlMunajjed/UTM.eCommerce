using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using Volo.Abp.Application.Services;

namespace UTM.eCommerce.Services
{
    public class CrudAppService : ApplicationService, ICrudAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public CrudAppService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Guid> CreateProductAsync(string name, decimal price, int stockCount)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                StockCount = stockCount
            };

            await _productRepository.InsertAsync(product, autoSave: true);

            return product.Id;
        }

        public async Task<Guid> PlaceOrderAsync(string customerFirstName, string customerLastName, string customerEmail, Guid productId, int quantity)
        {
            var customer = await GetOrCreateCustomerAsync(customerFirstName, customerLastName, customerEmail);

            var product = await _productRepository.GetAsync(productId);

            if (product == null)
            {
                throw new ApplicationException("Product not found.");
            }

            if (product.StockCount < quantity)
            {
                throw new ApplicationException("Insufficient stock.");
            }

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = product.Price * quantity,
                Customer = customer
            };

            product.StockCount -= quantity;
            product.Orders.Add(order);

            await _productRepository.UpdateAsync(product, autoSave: true);
            await _orderRepository.InsertAsync(order, autoSave: true);

            return order.Id;
        }

        private async Task<Customer> GetOrCreateCustomerAsync(string firstName, string lastName, string email)
        {
            var existingCustomer = await _customerRepository.FindByEmailAsync(email);

            if (existingCustomer != null)
            {
                return existingCustomer;
            }

            var newCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            await _customerRepository.InsertAsync(newCustomer, autoSave: true);

            return newCustomer;
        }
    }

}
