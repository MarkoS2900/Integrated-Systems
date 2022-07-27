using Cinema.Domain.DomainModels;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {

            _context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrders()
        {
            return entities.Include(z => z.productInOrders)
                .Include("productInOrders.product")
                .Include(z => z.user)
                .ToListAsync().Result;
        }

        public Order getOrder(Guid id)
        {
            return entities
                .Include(z => z.productInOrders)
                .Include("productInOrders.product")
                .Include(z => z.user)
                .SingleOrDefaultAsync(z => z.Id == id)
                .Result;
        }
    }
}
