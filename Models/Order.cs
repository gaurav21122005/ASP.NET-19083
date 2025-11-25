namespace RestaurantApp.Models
{
    public class Order
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public List<OrderItem> Items { get; set; } = new();

        public double TotalAmount { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
