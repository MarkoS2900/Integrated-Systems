using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Domain.DomainModels;
using Cinema.Domain.DTO;
using Cinema.Repository;
using Cinema.Services.Interface;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{

    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ApplicationDbContext _context, ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.getCartInfo(userId));
        }

        public IActionResult DeleteProductFromCart(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _cartService.deleteProductFromCart(userId, id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult PayForProducts(string stripeEmail, string stripeToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _cartService.getCartInfo(userId);
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (order.totalPrice * 100),
                Description = "Ticket Payment",
                Currency = "usd",
                Customer = customer.Id
            });
            if (charge.Status == "succeeded")
            {
                this.OrderProducts();
                return RedirectToAction("Index");
            }
            return null;
        }
        public bool OrderProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _cartService.OrderProductsAsync(userId);

            return true;

        }
    }
}
