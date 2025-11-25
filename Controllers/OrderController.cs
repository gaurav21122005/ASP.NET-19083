using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System.Security.Claims;
using Newtonsoft.Json;

namespace RestaurantApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel model)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var itemsList = JsonConvert.DeserializeObject<List<OrderItem>>(model.ItemsJson);

            if (itemsList == null || !itemsList.Any())
            {
                TempData["Error"] = "Cart is empty.";
                return RedirectToAction("Menu", "Home");
            }

            var order = new Order
            {
                UserId = userId!,
                Items = itemsList,
                TotalAmount = itemsList.Sum(i => i.Price * i.Quantity),
                Address = model.Address,
                Phone = model.Phone,
                PaymentMethod = model.PaymentMethod,
                Status = "Pending",
                OrderDate = DateTime.Now
            };

            await _orderService.PlaceOrderAsync(order);

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("OrderSuccess");
        }

        [HttpGet]
        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
