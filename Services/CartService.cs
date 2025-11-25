using MongoDB.Driver;
using RestaurantApp.Services;

public class CartService
{
    private readonly IMongoCollection<Cart> _carts;

    public CartService(MongoService mongo)
    {
        _carts = mongo.GetCollection<Cart>("Carts"); // <-- use GetCollection
    }


    public async Task<Cart?> GetCartByUserAsync(string userId) =>
        await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();

    public async Task AddToCartAsync(string userId, CartItem item)
    {
        var cart = await GetCartByUserAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId, Items = new List<CartItem> { item } };
            await _carts.InsertOneAsync(cart);
        }
        else
        {
            var existingItem = cart.Items.FirstOrDefault(i => i.MenuItemId == item.MenuItemId);
            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
                cart.Items.Add(item);

            await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
        }
    }

    public async Task ClearCartAsync(string userId) =>
        await _carts.DeleteOneAsync(c => c.UserId == userId);
}

public class Cart
{
    public string? Id { get; set; }
    public string UserId { get; set; } = null!;
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public string MenuItemId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
