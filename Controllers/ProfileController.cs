using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantApp.Services;

namespace RestaurantApp.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserService _userService;

    public ProfileController(UserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdAsync(userId!);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Update(string username, string email)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.UpdateAsync(userId!, username, email);
        return RedirectToAction("Index");
    }
}
