namespace UTM.eCommerce.Services.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public CustomerDto Customer { get; set; }
    }

}
