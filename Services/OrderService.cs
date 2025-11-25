using MongoDB.Driver;
using RestaurantApp.Models;

namespace RestaurantApp.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
            var db = client.GetDatabase("RestaurantDB");
            _orders = db.GetCollection<Order>("Orders");
        }

        public async Task PlaceOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orders.Find(x => x.UserId == userId).ToListAsync();
        }
    }
}
