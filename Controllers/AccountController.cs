using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Models;
using RestaurantApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace RestaurantApp.Controllers;

public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "All fields are required.";
            return View();
        }

        if (await _userService.GetByUsernameAsync(username) != null)
        {
            ViewBag.Error = "Username already taken.";
            return View();
        }

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = UserService.HashPassword(password)
        };

        await _userService.CreateAsync(user);
        await SignInUser(user);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string usernameOrEmail, string password)
    {
        if (await _userService.ValidateCredentialsAsync(usernameOrEmail, password))
        {
            var user = await _userService.GetByUsernameAsync(usernameOrEmail) ?? await _userService.GetByEmailAsync(usernameOrEmail);
            if (user != null)
            {
                await SignInUser(user);
                return RedirectToAction("Index", "Home");
            }
        }

        ViewBag.Error = "Invalid credentials.";
        return View();
    }

    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
