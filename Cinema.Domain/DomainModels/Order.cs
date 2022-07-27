using Cinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string userId { get; set; }
        public ApplicationUser user { get; set; }
        public virtual ICollection<ProductInOrder> productInOrders { get; set; }
    }
}
