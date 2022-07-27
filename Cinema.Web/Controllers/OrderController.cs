using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Cinema.Domain.Enums;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.getUser(userId);
            if (user.userRole == Roles.Standard)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            var orders = _orderService.getAllOrders();
            return View(orders);
        }

        public IActionResult Details(Guid id)
        {
            var order = _orderService.getOrder(id);
            return View(order);
        }
        public FileContentResult CreateInvoice(Guid id)
        {
            var order = _orderService.getOrder(id);
            var template = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var doc = DocumentModel.Load(template);
            doc.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            doc.Content.Replace("{{UserName}}", order.user.UserName);
            StringBuilder str = new StringBuilder();
            float totPrice = 0;
            foreach (var item in order.productInOrders)
            {
                str.AppendLine(item.product.productName);
                totPrice += item.quantity * item.product.productPrice;
            }
            doc.Content.Replace("{{ProductList}}", str.ToString());
            doc.Content.Replace("{{TotalPrice}}", totPrice.ToString(CultureInfo.InvariantCulture) + "$");
            var stream = new MemoryStream();
            doc.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        public IActionResult MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.getUser(userId);
            var ordersFromCurrentUser = _orderService.getAllOrders().Where(z => z.user.Equals(user)).ToList();
            return View(ordersFromCurrentUser);
        }
    }
}
