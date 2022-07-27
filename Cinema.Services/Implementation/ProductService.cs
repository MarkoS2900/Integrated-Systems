using Cinema.Domain.DomainModels;
using Cinema.Domain.DTO;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<ProductInCart> _productInCartRepository;

        public ProductService(IRepository<Product> productRepository, IUserRepository userRepository, IRepository<ProductInCart> productInCartRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInCartRepository = productInCartRepository;
        }

        public bool AddToShoppingCart(AddToCartDTO item, string userID)
        {
            var user = this._userRepository.GetUser(userID);

            var userShoppingCard = user.userCart;

            if (item.product != null && userShoppingCard != null)
            {
                return AddProductInCart(item, userShoppingCard);
            }
            return false;
        }
        public bool AddProductInCart(AddToCartDTO item, ShoppingCart cart)
        {
            var product = GetDetailsForProduct(item.productId);
            if (product != null)
            {
                ProductInCart itemToAdd = new ProductInCart
                {
                    Id = Guid.NewGuid(),
                    product = product,
                    productId = product.Id,
                    shoppingCart = cart,
                    shoppingCartID = cart.Id,
                    quantity = item.quantity
                };
                _productInCartRepository.Insert(itemToAdd);
                return true;
            }
            return false;
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return _productRepository.Get(id);
        }

        public void CreateNewProduct(Product p)
        {
            this._productRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = GetDetailsForProduct(id);
            _productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            return this._productRepository.GetAll().ToList();
        }


        public AddToCartDTO GetShoppingCartInfo(Guid? id)
        {
            var product = GetDetailsForProduct(id);
            AddToCartDTO model = new AddToCartDTO
            {
                product = product,
                productId = product.Id,
                quantity = 1
            };
            return model;
        }

        public void UpdeteExistingProduct(Product p)
        {
            _productRepository.Update(p);
        }
    }
}
