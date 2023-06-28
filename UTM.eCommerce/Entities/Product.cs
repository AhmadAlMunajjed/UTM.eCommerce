using Volo.Abp.Domain.Entities.Auditing;

namespace UTM.eCommerce.Entities
{
    public class Product : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Product()
        {
            Orders = new List<Order>();
        }
    }
}
