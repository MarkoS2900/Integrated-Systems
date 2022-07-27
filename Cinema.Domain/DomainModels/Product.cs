using Cinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        [Display(Name = "Product")]
        public string productName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string productDescription { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int productPrice { get; set; }

        [Required]
        [Display(Name = "Valid until:")]
        [DataType(DataType.Date)]
        public DateTime validUntil { get; set; }

        [Required]
        [EnumDataType(typeof(Genres))]
        public Genres genre { get; set; }
        public virtual ICollection<ProductInCart> productsInCart { get; set; }
        public virtual ICollection<ProductInOrder> productInOrders { get; set; }

    }
}
