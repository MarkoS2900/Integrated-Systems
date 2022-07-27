using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Domain.Enums;
using Cinema.Domain.DomainModels;
using Cinema.Domain.DTO;
using Cinema.Repository;
using Cinema.Services.Interface;

namespace Cinema.Web.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public ProductsController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        // GET: Products
        public IActionResult Index()
        {
            var allProducts = _productService.GetAllProducts();
            return View(allProducts);
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.getUser(userId);
            if (user.userRole.Equals(Roles.Administrator))
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,productName,productDescription,productPrice,validUntil,Genre")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                _productService.CreateNewProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("id,productName,productDescription,productPrice")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productService.UpdeteExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _productService.GetDetailsForProduct(id) != null;
        }

        public IActionResult AddProductToCart(Guid? id)
        {
            var model = _productService.GetShoppingCartInfo(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult AddProductToCart(AddToCartDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = _productService.GetDetailsForProduct(model.productId);
            model.product = product;
            var result = _productService.AddToShoppingCart(model, userId);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(model);

        }

        public IActionResult ExportTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.getUser(userId);
            if (user.userRole.Equals(Roles.Administrator))
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Home");

        }

        [HttpPost]
        public IActionResult ExportTickets([Bind("Genre")] Product product)
        {
            Genres genre = product.genre;
            var productsOfGenre = _productService.GetAllProducts().Where(z => z.genre.Equals(genre)).ToList();
            string fileName = $"Tickets of genre {genre.ToString()}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Tickets");
                worksheet.Cell(1, 1).Value = "Ticket Id";
                worksheet.Cell(1, 2).Value = "Ticket title";
                worksheet.Cell(1, 3).Value = "Ticket price";
                worksheet.Cell(1, 4).Value = "Valid until";
                worksheet.Cell(1, 5).Value = "Genre";

                for (int i = 1; i <= productsOfGenre.Count(); i++)
                {
                    worksheet.Cell(i + 1, 1).Value = productsOfGenre[i - 1].Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = productsOfGenre[i - 1].productName.ToString();
                    worksheet.Cell(i + 1, 3).Value = $"{productsOfGenre[i - 1].productPrice.ToString()}$";
                    worksheet.Cell(i + 1, 4).Value = productsOfGenre[i - 1].validUntil.ToString();
                    worksheet.Cell(i + 1, 5).Value = productsOfGenre[i - 1].genre.ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }



    }
}
