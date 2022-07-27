using Cinema.Domain.DomainModels;
using Cinema.Domain.DTO;
using Cinema.Domain.Identity;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ShoppingCart> _cartRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _emailRepository;
        private readonly IEmailService _emailService;

        public CartService(IRepository<Order> orderRepository, IRepository<ShoppingCart> cartRepository, IRepository<ProductInOrder> productInOrderRepository, IUserRepository userRepository, IRepository<EmailMessage> emailRepository, IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productInOrderRepository = productInOrderRepository;
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        public bool deleteProductFromCart(string userId, Guid? id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var userCart = GetShoppingCart(userId);
                var productToRemove = userCart.productsInCart.Where(z => z.productId.Equals(id)).FirstOrDefault();
                userCart.productsInCart.Remove(productToRemove);
                _cartRepository.Update(userCart);
                return true;
            }
            return false;
        }

        public ShoppingCart GetShoppingCart(string id)
        {
            return _userRepository.GetUser(id).userCart;
        }

        public CartDTO getCartInfo(string id)
        {
            var userCart = GetShoppingCart(id);
            var productList = userCart.productsInCart.Select(z => new
            {
                quantity = z.quantity,
                productPrice = z.product.productPrice
            }).ToList();

            int totPrice = 0;
            foreach (var item in productList)
            {
                totPrice += item.productPrice * item.quantity;
            }
            CartDTO model = new CartDTO
            {
                totalPrice = totPrice,
                productsInCart = userCart.productsInCart.ToList()
            };
            return model;
        }

        public async Task OrderProductsAsync(string id) //ne znam shto
        {
            if (!string.IsNullOrEmpty(id))
            {
                List<EmailMessage> emails = new List<EmailMessage>();
                var user = this._userRepository.GetUser(id);
                var userCart = user.userCart;
                EmailMessage email = new EmailMessage();
                email = SetEmailValues(email, user);

                Order order = new Order
                {
                    userId = user.Id,
                    user = user
                };
                _orderRepository.Insert(order);
                List<ProductInOrder> productsInOrder = new List<ProductInOrder>();
                var productInOrders = userCart.productsInCart.Select(z => new ProductInOrder
                {
                    productId = z.productId,
                    orderId = order.Id,
                    product = z.product,
                    order = order,
                    quantity = z.quantity
                });
                productsInOrder.AddRange(productInOrders);
                StringBuilder str = new StringBuilder();
                str.AppendLine("Order successful. Your ordered products:");
                float totalPrice = 0;
                foreach (var item in productsInOrder)
                {
                    str.AppendLine($"{item.quantity} x {item.product.productName}, product price: {item.product.productPrice}$");
                    totalPrice += (item.product.productPrice * item.quantity);
                    _productInOrderRepository.Insert(item);
                }
                user.userCart.productsInCart.Clear();
                _userRepository.UpdateUser(user);
                str.AppendLine($"Total price: {totalPrice}$");
                email.content = str.ToString();
                _emailRepository.Insert(email);
                emails.Add(email);
                await _emailService.SendEmailAsync(emails);
            }
            else
            {
                throw new ArgumentNullException(id);
            }
        }

        public EmailMessage SetEmailValues(EmailMessage email, ApplicationUser user)
        {
            email.mailTo = user.Email;
            email.subject = "Order successful";
            email.status = true;
            return email;
        }
    }
}
