using Contracts.Request.OrderRequests;
using Contracts.Request.PaymentRequests;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Mvc;
using ShredStorePresentation.Extensions.Cache;
using ShredStorePresentation.Models;
using ShredStorePresentation.Services.CartItemServices;
using ShredStorePresentation.Services.CartServices;
using ShredStorePresentation.Services.ProductServices;
using ShredStorePresentation.Services.UserService;

namespace ShredStorePresentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartHttpService _cart;
        private readonly ICartItemHttpService _cartItem;
        private readonly IProductHttpService _product;
        private readonly IUserHttpService _userHttpService;

        public OrderController(ICartHttpService cart, ICartItemHttpService cartItem, IProductHttpService product, IUserHttpService userHttpService)
        {
            _cart = cart;
            _cartItem = cartItem;
            _product = product;
            _userHttpService = userHttpService;
        }
        public async Task<IActionResult> Payment(CancellationToken token)
        {
            int userId = HttpContext.Session.GetInt32(SessionKeys.GetSessionKeyId())!.Value;

            IEnumerable<ProductCartItemResponse> result = await _product.GetAllByCartId(userId, token);

            UserResponse user = await _userHttpService.GetById(userId);

            CreateOrderViewRequest request = new CreateOrderViewRequest
            {
                Address = user.Address,
                Name = user.Name,
                CreatedDate = DateTime.Now,
                Email = user.Email,
                UserId = userId,
                TotalAmount = GetTotalPrice(result)
            };

            return View(request);
        }

        private decimal GetTotalPrice(IEnumerable<ProductCartItemResponse> result)
        {
            decimal total = 0m;
            foreach (var product in result)
                total += product.Price;

            return total;
        }
    }
}
