using Contracts.Response.ProductsResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Services.ProductServices;


namespace ShredStorePresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IProductHttpService _productService;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache, IProductHttpService product)
        {
            _logger = logger;
            _cache = cache;
            _productService = product;
        }


        public async Task<IActionResult> Index(string Search = "")
        {
            string recordKey = "Products_";
            try
            {
                var allProducts = await GetAllProducts(recordKey);
                if (Search == "" || Search is null)
                {
                    return View(allProducts);
                }
                else
                {
                    var list = SearchResults(Search, allProducts);
                    ViewBag.Search = "Ok";
                    return View(list);
                }
            }
            catch (Exception ex)
            {
                //_utilityClass.GetLog().Error(ex, "Exception caught at Index action in ShredStoreController.");
            }
            return View();
        }
        private async Task<IEnumerable<ProductResponse>> GetAllProducts(string recordKey)
        {
            var products = await _cache.GetRecordAsync<IEnumerable<ProductResponse>>(recordKey);
            if (products is null)
            {
                var getProducts = await _productService.GetAll();
                SetOnCache(recordKey, getProducts);
                return getProducts;
            }
            return products;
        }
        private async Task<IEnumerable<ProductResponse>> GetCategoryProducts(string recordKey, string category)
        {
            var products = await _cache.GetRecordAsync<IEnumerable<ProductResponse>>(recordKey);
            if (products is null)
            {
                var getProducts = await _productService.GetAllByCategory(category);
                SetOnCache(recordKey, getProducts);
                return getProducts;
            }
            return products;
        }

        public async Task<IActionResult> Category(string Category)
        {
            string recordKey = $"{Category}_";
            var products = await GetCategoryProducts(recordKey, Category);
            try
            {
                ViewBag.Title = Category;
                return View(products);
            }
            catch (Exception ex)
            {
                //_utilityClass.GetLog().Error(ex, "Exception caught at Category action in ShredStoreController.");
            }
            return View();

        }
        public async Task<IActionResult> EmptyCart()
        {
            ViewBag.NoProds = "True";
            ViewBag.Message = "No products in cart!";
            string recordKey = "Products_";
            try
            {
                var products = await GetAllProducts(recordKey);
                return View("Index", products);
            }
            catch (Exception ex)
            {
                //_utilityClass.GetLog().Error(ex, "Exception caught at EmptyCart action in ShredStoreController.");
            }
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<ProductResponse> SearchResults(string search, IEnumerable<ProductResponse> products)
        {
            var searchResults = products.Where(p => p.Name.Contains(search) || p.Category.Contains(search) || p.Brand.Contains(search))
                         .OrderBy(p => p.Name);
            return searchResults;
        }
        private async void SetOnCache(string recordKey, IEnumerable<ProductResponse> products)
        {
            await _cache.SetRecordAsync(recordKey, products, TimeSpan.FromSeconds(35));
        }
        public IActionResult AboutUs()
        {
            return View();
        }


    }
}
