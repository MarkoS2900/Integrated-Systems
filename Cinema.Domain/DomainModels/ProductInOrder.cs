using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class ProductInOrder : BaseEntity
    {
        public Guid productId { get; set; }
        public Guid orderId { get; set; }
        public Product product { get; set; }
        public Order order { get; set; }
        public int quantity { get; set; }

    }
}
