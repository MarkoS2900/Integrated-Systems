using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Domain.Enums;
using Cinema.Domain.Identity;
using Cinema.Repository;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cinema.Domain.DomainModels;

namespace Cinema.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _userService.getUser(userId);
            if (currentUser.userRole.Equals(Roles.Administrator))
            {
                var allUsersExceptCurrentUser = _userService.getAllUsers().Where(a => a.Id != currentUser.Id).ToList();
                return View(allUsersExceptCurrentUser);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home", new { area = "" });
            }

        }

        public IActionResult ChangeRoleToAdministrator(string id)
        {

            var user = _userService.getUser(id);
            if (user.userRole != Roles.Administrator)
            {
                user.userRole = Roles.Administrator;
                _userService.UpdateUser(user);
            }
            return RedirectToAction("Index");

        }
        public IActionResult ChangeRoleToStandard(string id)
        {
            var user = _userService.getUser(id);
            if (user.userRole != Roles.Standard)
            {
                user.userRole = Roles.Standard;
                _userService.UpdateUser(user);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ImportFile()
        {
            return View();
        }

        public IActionResult ImportUsers(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
            using (FileStream fs = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            List<UserRegistrationDTO> users = getUsersFromFile(file.FileName);

            foreach (var item in users)
            {
                var userCheck = _userManager.FindByEmailAsync(item.email).Result;
                if (userCheck == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = item.email,
                        NormalizedUserName = item.email,
                        UserName = item.email,
                        userRole = item.userRole,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        userCart = new ShoppingCart()

                    };
                    var result = _userManager.CreateAsync(user, item.password).Result;
                }
                else
                {
                    continue;
                }
            }
            return RedirectToAction("Index");
        }

        public List<UserRegistrationDTO> getUsersFromFile(string filename)
        {
            List<UserRegistrationDTO> users = new List<UserRegistrationDTO>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{filename}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var str = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var r = ExcelReaderFactory.CreateReader(str))
                {
                    while (r.Read())
                    {
                        users.Add(new UserRegistrationDTO
                        {
                            email = r.GetValue(0).ToString(),
                            password = r.GetValue(1).ToString(),
                            confirmPassword = r.GetValue(2).ToString(),
                            userRole = r.GetValue(3).ToString().Equals("Standard") ? Roles.Standard : Roles.Administrator
                        });

                    }
                }
            }
            return users;
        }
    }
}
