using Contracts.Request.CartItemRequests;
using Contracts.Request.CartRequests;
using Contracts.Response.ProductsResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStorePresentation.Services.CartItemServices;
using ShredStorePresentation.Services.CartServices;
using ShredStorePresentation.Services.ProductServices;

namespace ShredStorePresentation.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartHttpService _cart;
        private readonly ICartItemHttpService _cartItem;
        private readonly IProductHttpService _product;


        public CartController(ICartHttpService cart, ICartItemHttpService cartItem, IProductHttpService product)
        {
            _cart = cart;
            _cartItem = cartItem;
            _product = product;
        }

        public async Task<IActionResult> InsertToCart(int productId, int quantity, CancellationToken token)
        {

            if (HttpContext.Session.GetInt32("_Id") is null)
                return RedirectToAction("Login", "User");

            int userId = HttpContext.Session.GetInt32("_Id")!.Value;

            var cart = await _cart.GetById(userId, token);

            if (cart is null || cart.UserId == 0)
            {
                CreateCartRequest request = new CreateCartRequest
                {
                    CreatedDate = DateTime.Now,
                    UserId = userId
                };
                await _cart.Create(request, token);
                cart = await _cart.GetById(userId, token);
            }

            CreateCartItemRequest cartItemRequest = new CreateCartItemRequest
            {
                CartId = cart.UserId,
                ProductId = productId,
                Quantity = quantity

            };

            await _cartItem.InsertCartItems(cartItemRequest, token);

            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> ChangeQuantity(string quantity, string productId, CancellationToken token)
        {

            int qtd = Convert.ToInt32(quantity);
            int Id = Convert.ToInt32(productId);
            int userId = HttpContext.Session.GetInt32("_Id")!.Value;

            UpdateCartItemRequest request = new UpdateCartItemRequest
            {
                CartId = userId,
                Quantity = qtd,
                ProductId = Id
            };

            await _cartItem.UpdateCartItem(request, token);
            return await CartItems(default);
        }
        public async Task<IActionResult> CartItems(CancellationToken token)
        {
            int userId = HttpContext.Session.GetInt32("_Id")!.Value;

            IEnumerable<ProductCartItemResponse> result = await _product.GetAllByCartId(userId, token);

            if (!result.Any())
                return RedirectToAction("EmptyCart", "Home");

            ViewBag.TotalPrice = GetTotalPrice(result);

            return View("CartItems", result);
        }
        public async Task<IActionResult> RemoveCartItem(int productId)
        {
            int userId = HttpContext.Session.GetInt32("_Id")!.Value;
            await _cartItem.RemoveItem(productId, userId);
            return await CartItems(default);
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
