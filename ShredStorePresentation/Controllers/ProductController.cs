﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Models;
using ShredStorePresentation.Services.Images;
using ShredStorePresentation.Services.ProductServices;

namespace ShredStorePresentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductHttpService _product;
        private readonly IDistributedCache _cache;
        private readonly IImageService _imageService;
        
        
        public ProductController(IProductHttpService _product,
                                 IDistributedCache _cache,
                                 IImageService imageService)
        {
            this._product = _product;
            this._cache = _cache;
            _imageService = imageService;
        }
        public async Task<IActionResult> ProductDetails(int Id, CancellationToken token)
        {
            try
            {
                var selected = await _product.GetById(Id, token);
                return View(selected);
            }
            catch (Exception ex)
            {
                //_utilityClass.GetLog().Error(ex, "Exception caught at ProductDetails action in ShredStoreController.");
                return RedirectToAction(nameof(Index), "Home");
            }

        }
    
        // GET: ShredStoreController/Create
        public IActionResult PublishProduct()
        {
            SetProductDropdowns();
            return View();
        }



        // POST: ShredStoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishProduct(CreateProductViewRequest productInfo, CancellationToken token)
        {
            SetProductDropdowns();            
            if (ModelState.IsValid)
            {
                try
                {
                    productInfo.ImageName = await _imageService.UploadImage(productInfo.ImageFile);
                    await _product.Create(productInfo.MapToProductRequest(), token);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //_utilityClass.GetLog().Error(ex, "Exception caught at PublishProduct action in ShredStoreController.");
                    
                    return View();
                }

            }
            
            return View();
        }
        // GET: ShredStoreController/Edit/5
        public async Task<IActionResult> EditProduct(int id, CancellationToken token)
        {
            var selected = await _product.GetById(id, token);
            SetProductDropdowns();
            return View(selected.MapToUpdateProductRequest());
        }

        public async Task<IActionResult> EditProduct(UpdateProductViewRequest edited, CancellationToken token)
        {
            SetProductDropdowns();
            ModelState.Remove("ImageFile");
            if (ModelState.IsValid)
            {
                try
                {
                    if (edited.ImageFile is not null)
                    {
                        _imageService.DeleteImage(edited.ImageName);
                        await _imageService.UploadImage(edited.ImageFile);
                    }                     
                    
                    await _product.Edit(edited.MapToUpdateProductRequest(), token);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    //_utilityClass.GetLog().Error(ex, "Exception caught at EditProduct action in ShredStoreController.");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteProduct(int id, CancellationToken token)
        {
            try
            {
                var selected = await _product.GetById(id, token);
                
                bool result = _imageService.DeleteImage(selected.ImageName);
                
                await _product.Delete(selected.Id, token);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //_utilityClass.GetLog().Error(ex, "Exception caught at DeleteProduct action in ShredStoreController.");
                return View();
            }
        }
        public async Task<IActionResult> AboutUs() => View();
        private List<string> Categories()
        {
            List<string> categories =
            [
                "Eletric Guitar",
                "Pedals",
                "Amplifier",
                "Accessories",
                "Acoustic Guitar",
            ];
            return categories;
        }
        private List<string> Types()
        {
            List<string> categories =
            [
                "6 Strings",
                "7 Strings",
                "8 Strings",
                "12 Strings"
            ];
            return categories;
        }
        private void SetProductDropdowns()
        {
            ViewBag.Categories = new SelectList(Categories());
            ViewBag.Types = new SelectList(Types());
        }

    }
}
