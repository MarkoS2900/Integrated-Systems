using Cinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Services.Interface
{
    public interface IOrderService
    {
        public List<Order> getAllOrders();

        public Order getOrder(Guid id);
    }
}
