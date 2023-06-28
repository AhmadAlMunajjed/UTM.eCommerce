using Castle.Core.Resource;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace UTM.eCommerce.Entities
{
    public class Order : FullAuditedEntity<Guid>
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Product> Products { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }
    }

}
