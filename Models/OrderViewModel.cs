using System.Collections.Generic;

namespace RestaurantApp.Models
{
    public class OrderViewModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        // Cart items for checkout page
        public List<OrderItem> Items { get; set; } = new();

        // Total amount calculation
        public double TotalAmount { get; set; }

        // For JSON passed from hidden field
        public string ItemsJson { get; set; } = string.Empty;
    }
}
