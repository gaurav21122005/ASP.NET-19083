using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantApp.Models;

public class MenuItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // "Pizza", "Burger", "Dessert"
}
