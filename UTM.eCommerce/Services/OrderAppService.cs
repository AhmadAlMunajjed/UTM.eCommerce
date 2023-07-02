using System.Diagnostics;
using UTM.eCommerce.Entities;
using UTM.eCommerce.Repositories;
using UTM.eCommerce.Services.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UTM.eCommerce.Services
{
    public class OrderAppService : ApplicationService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        
        public OrderAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<Order, Guid> orderRepository,
            IRepository<Customer, Guid> customerRepository)
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

        public async Task<Guid> CreateCustomerAsync(string firstName, string lastName, string email)
        {
            var newCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var created = await _customerRepository.InsertAsync(newCustomer);
            return created.Id;
        }

        public async void PlaceOrder(PlaceOrderDto input)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
        }
    }

}

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
