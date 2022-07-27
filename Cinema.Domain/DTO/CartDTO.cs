using Cinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DTO
{
    public class CartDTO
    {
        public List<ProductInCart> productsInCart { get; set; }
        public int totalPrice { get; set; }
    }
}
