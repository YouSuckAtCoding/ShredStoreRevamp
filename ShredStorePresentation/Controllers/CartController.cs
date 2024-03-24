using Microsoft.AspNetCore.Mvc;

namespace ShredStorePresentation.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartHttpService _cart;

        public CartController(ICartHttpService cart)
        {
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
