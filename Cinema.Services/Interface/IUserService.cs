using Cinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Services.Interface
{
    public interface IUserService
    {
        public ApplicationUser getUser(string id);
        public List<ApplicationUser> getAllUsers();
        public ApplicationUser getUserByEmail(string email);
        public bool UpdateUser(ApplicationUser user);
        public Task SeedAdministrator();
    }
}
