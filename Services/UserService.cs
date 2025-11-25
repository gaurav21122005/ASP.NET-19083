using MongoDB.Driver;
using RestaurantApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantApp.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(MongoService mongo)
    {
        _users = mongo.GetCollection<User>("Users");

    }

    // Get user by username
    public async Task<User?> GetByUsernameAsync(string username) =>
        await _users.Find(u => u.Username.ToLower() == username.ToLower()).FirstOrDefaultAsync();

    // Get user by email
    public async Task<User?> GetByEmailAsync(string email) =>
        await _users.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

    // Get user by ID
    public async Task<User?> GetByIdAsync(string id) =>
        await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

    // Create new user
    public async Task CreateAsync(User user) =>
        await _users.InsertOneAsync(user);

    // Update user info
    public async Task UpdateAsync(string id, string username, string email)
    {
        var update = Builders<User>.Update
            .Set(u => u.Username, username)
            .Set(u => u.Email, email);

        await _users.UpdateOneAsync(u => u.Id == id, update);
    }

    // Password hashing
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    // Validate login credentials
    public async Task<bool> ValidateCredentialsAsync(string usernameOrEmail, string password)
    {
        var user = await _users.Find(u =>
            u.Username.ToLower() == usernameOrEmail.ToLower() ||
            u.Email.ToLower() == usernameOrEmail.ToLower()).FirstOrDefaultAsync();

        if (user == null) return false;

        return user.PasswordHash == HashPassword(password);
    }
}
