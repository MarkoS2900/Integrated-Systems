using Cinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DTO
{
    public class AddToCartDTO
    {
        public Product product { get; set; }
        public Guid productId { get; set; }
        public int quantity { get; set; }
    }
}
