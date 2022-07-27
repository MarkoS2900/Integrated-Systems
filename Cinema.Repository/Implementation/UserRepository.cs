using Cinema.Domain.Identity;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<ApplicationUser> entities;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ApplicationUser>();
        }
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return entities.AsEnumerable();
        }

        public ApplicationUser GetUser(string id)
        {
            return entities
               .Include(z => z.userCart)
               .Include("userCart.productsInCart")
               .Include("userCart.productsInCart.product")
               .SingleOrDefault(s => s.Id.Equals(id));
        }

        public void CheckIsEntityNull(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void InsertUser(ApplicationUser entity)
        {
            CheckIsEntityNull(entity);
            entities.Add(entity);
            context.SaveChanges();
        }

        public void UpdateUser(ApplicationUser entity)
        {
            CheckIsEntityNull(entity);
            entities.Update(entity);
            context.SaveChanges();
        }

        public void DeleteUser(ApplicationUser entity)
        {
            CheckIsEntityNull(entity);
            entities.Remove(entity);
            context.SaveChanges();
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return entities.SingleOrDefault(s => s.Email.Equals(email));
        }
    }
}
