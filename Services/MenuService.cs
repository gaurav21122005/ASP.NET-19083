using MongoDB.Driver;
using RestaurantApp.Models;

namespace RestaurantApp.Services;

public class MenuService
{
    private readonly IMongoCollection<MenuItem> _menu;

    public MenuService(MongoService mongo)
    {
        _menu = mongo.GetCollection<MenuItem>("MenuItems");
    }


    public async Task<List<MenuItem>> GetAllAsync() =>
        await _menu.Find(_ => true).ToListAsync();

    public async Task<List<MenuItem>> GetByCategoryAsync(string category) =>
        await _menu.Find(m => m.Category.ToLower() == category.ToLower()).ToListAsync();

    public async Task AddMenuItemAsync(MenuItem item) =>
        await _menu.InsertOneAsync(item);
}
