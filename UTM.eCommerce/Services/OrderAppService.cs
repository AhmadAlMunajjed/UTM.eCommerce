using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using UTM.eCommerce.Services.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Services
{
    public class OrderAppService : ApplicationService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        public class CreateProductDto
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int StockCount { get; set; }
            public string CustomerFirstName { get; set; }
            public string CustomerLastName { get; set; }
            public string CustomerEmail { get; set; }
        }

        public class PlaceOrderDto
        {
            public Guid CustomerId { get; set; }
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }

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

        private async Task<Customer> GetOrCreateCustomer(string firstName, string lastName, string email)
        {
            var customers  = await _customerRepository.GetListAsync();

            if (customers.Count == 0)
            {
                throw new ApplicationException("No customers found.");
            }

            var customer = customers.FirstOrDefault(c => c.FirstName == firstName && c.LastName == lastName);

            if (customer == null)
            {
                customer = await CreateCustomerAsync(firstName, lastName, email);
            }

            return customer;
        }

        private async Task<Customer> CreateCustomerAsync(string firstName, string lastName, string email)
        {
            var newCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
            await _customerRepository.InsertAsync(newCustomer);
            return newCustomer;
        }

        public async void PlaceOrder(PlaceOrderDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == input.ProductId);
            if (product == null)
            {
                throw new ApplicationException("Product not found.");
            }

            var customer = await _customerRepository.FirstOrDefaultAsync(c => c.Id == input.CustomerId);
            if (customer == null)
            {
                throw new ApplicationException("Customer not found.");
            }

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = product.Price * input.Quantity,
                Customer = customer
            };

            product.StockCount -= input.Quantity;

            customer.Orders.Add(order);

            await _productRepository.UpdateAsync(product);
            await _orderRepository.InsertAsync(order);
        }
    }

}
