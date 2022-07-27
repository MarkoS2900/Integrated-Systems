using Microsoft.AspNetCore.Identity;
using Cinema.Domain.DomainModels;
using Cinema.Domain.Identity;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Services.Implementation
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public ApplicationUser getUser(string id)
        {
            var user = _userRepository.GetUser(id);
            return user;
        }

        public List<ApplicationUser> getAllUsers()
        {
            return _userRepository.GetAllUsers().ToList();
        }

        public bool UpdateUser(ApplicationUser user)
        {
            if (user != null)
            {
                _userRepository.UpdateUser(user);
                return true;
            }
            return false;
        }
        public ApplicationUser getUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public async Task SeedAdministrator()
        {
            var user = new ApplicationUser
            {
                firstName = "Admin",
                lastName = "Admin",
                UserName = "administrator@test.com",
                NormalizedUserName = "administrator@test.com",
                Email = "administrator@test.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                userRole = Domain.Enums.Roles.Administrator,
                userCart = new ShoppingCart()
            };
            var u = _userRepository.GetAllUsers().Where(z => z.Email.Equals(user.Email)).ToList();
            if (u.Count() == 0)
            {
                var result = await _userManager.CreateAsync(user, "Test123.");
            }
        }
    }
}
