using Contracts.Response.ProductsResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Extensions.Cache;
using ShredStorePresentation.Services.ProductServices;


namespace ShredStorePresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IProductHttpService _productService;
        private readonly CacheRecordKeys _cachekeys;
        
        public HomeController(ILogger<HomeController> logger, IDistributedCache cache, IProductHttpService product, CacheRecordKeys cachekeys)
        {
            _logger = logger;
            _cache = cache;
            _productService = product;
            _cachekeys = cachekeys;
        }


        public async Task<IActionResult> Index(CancellationToken token ,string Search = "")
        {
            
            try
            {
                var res = ControllerExtensions.ControllerActionName<HomeController>();

                var allProducts = await GetAllProducts(_cachekeys.GetProductCacheKey());
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
                _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
            }
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public async Task<IActionResult> Category(string Category, CancellationToken token)
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
               _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
            }
            return View();

        }
        public async Task<IActionResult> EmptyCart(CancellationToken token)
        {
            ViewBag.NoProds = "True";
            ViewBag.Message = "No products in cart!";
            try
            {
                var products = await GetAllProducts(_cachekeys.GetProductCacheKey());
                return View(nameof(Index), products);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
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


    }
}
