using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Models;
using RestaurantApp.Services;

namespace RestaurantApp.Controllers;

public class HomeController : Controller
{
    private readonly MenuService _menuService;

    public HomeController(MenuService menuService)
    {
        _menuService = menuService;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Menu page with optional category filter
    public async Task<IActionResult> Menu(string? category)
    {
        List<MenuItem> menuItems;

        if (!string.IsNullOrEmpty(category))
        {
            menuItems = await _menuService.GetByCategoryAsync(category);
        }
        else
        {
            menuItems = await _menuService.GetAllAsync();
        }

        ViewBag.Category = category ?? "All";
        return View(menuItems);
    }
}
