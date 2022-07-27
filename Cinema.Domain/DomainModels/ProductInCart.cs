using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class ProductInCart : BaseEntity
    {
        public Guid productId { get; set; }
        public Product product { get; set; }
        public Guid shoppingCartID { get; set; }
        public ShoppingCart shoppingCart { get; set; }
        public int quantity { get; set; }


    }
}
