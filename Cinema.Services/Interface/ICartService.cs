using Cinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Services.Interface
{
    public interface ICartService
    {
        public CartDTO getCartInfo(string id);
        public bool deleteProductFromCart(string userId, Guid? id);

        public Task OrderProductsAsync(string id);


    }
}
