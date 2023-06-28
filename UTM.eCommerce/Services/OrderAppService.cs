using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using UTM.eCommerce.Services.Dtos;
using Volo.Abp.Application.Services;

namespace UTM.eCommerce.Services
{
    public class OrderAppService : ApplicationService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderAppService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<OrderDto> GetOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            return ObjectMapper.Map<Order, OrderDto>(order);
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
                CustomerId = customer.Id
            };

            product.StockCount -= quantity;
            product.Orders.Add(order);

            await _productRepository.UpdateAsync(product);
            await _orderRepository.InsertAsync(order);

            return order.Id;
        }

        private async Task<Customer> GetOrCreateCustomerAsync(string firstName, string lastName, string email)
        {
            var customer = await _customerRepository.FindByEmailAsync(email);
            if (customer != null)
            {
                return customer;
            }

            customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            await _customerRepository.InsertAsync(customer);

            return customer;
        }
    }

}
