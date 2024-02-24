using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Models;
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
        private async Task<IEnumerable<ProductViewResponse>> GetAllProducts(string recordKey)
        {
            var products = await _cache.GetRecordAsync<IEnumerable<ProductViewResponse>>(recordKey);
            if (products is null)
            {
                var getProducts = await _productService.GetAll();
                SetOnCache(recordKey, getProducts);
                return getProducts;
            }
            return products;
        }
        private async Task<IEnumerable<ProductViewResponse>> GetCategoryProducts(string recordKey, string category)
        {
            var products = await _cache.GetRecordAsync<IEnumerable<ProductViewResponse>>(recordKey);
            if (products is null)
            {
                var getProducts = await _productService.GetAllByCategory(category);
                SetOnCache(recordKey, getProducts);
                return getProducts;
            }
            return products;
        }
        private IEnumerable<ProductViewResponse> SearchResults(string search, IEnumerable<ProductViewResponse> products)
        {
            var searchResults = products.Where(p => p.Name.Contains(search) || p.Category.Contains(search) || p.Brand.Contains(search))
                         .OrderBy(p => p.Name);
            return searchResults;
        }
        private async void SetOnCache(string recordKey, IEnumerable<ProductViewResponse> products)
        {
            await _cache.SetRecordAsync(recordKey, products, TimeSpan.FromSeconds(35));
        }
        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
