using Cinema.Domain.DomainModels;
using Cinema.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Roles userRole { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public virtual ShoppingCart userCart { get; set; }

    }
}
