using Cinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string ownerId { get; set; }
        public ApplicationUser owner { get; set; }
        public virtual ICollection<ProductInCart> productsInCart { get; set; }

    }
}
