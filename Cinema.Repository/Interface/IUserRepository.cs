using Cinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> GetAllUsers();
        ApplicationUser GetUser(string id);
        void InsertUser(ApplicationUser entity);
        void UpdateUser(ApplicationUser entity);
        void DeleteUser(ApplicationUser entity);
        public ApplicationUser GetUserByEmail(string email);
    }
}
