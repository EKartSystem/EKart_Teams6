using E_Kart_Application.DTOs.ProductsDTO;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace EKartMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductApiService _service;

        public ShopController(ProductApiService service)
        {
            _service = service;
        }
        //public async Task<IActionResult> Index(string? name, bool? showExpensive=null)
        //{
        //    var token = HttpContext.Session.GetString("jwt");
        //    IEnumerable<ProductListingDto> products;
        //    if (showExpensive.HasValue && showExpensive.Value)
        //    {
        //        var expensiveData = await _service.GetExpensiveProductAsync(token);
        //        products = expensiveData.Select(e => new ProductListingDto
        //        {
        //            ProductId = e.ProductId, 
        //            ProductName = e.ProductName,
        //            UnitPrice = e.UnitPrice,
        //            CategoryName = e.CategoryName ?? "Premium",
        //            IsInStock = e.UnitsInStock > 0
        //        }).ToList();
        //        return View(products);
        //    }

        //    if (string.IsNullOrWhiteSpace(name))
        //    {
        //        products = await _service.GetDataAsync(token);
        //    }
        //    else
        //    {
        //        products = await _service.GetProductByName(name,token);
        //    }
        //    return View(products);
        //}

        public async Task<IActionResult> Index(string? name, bool? showExpensive = null, int pageNumber = 1)
        {
            int pageSize = 8; 
            var token = HttpContext.Session.GetString("jwt");
            IEnumerable<ProductListingDto> products = new List<ProductListingDto>();
            int totalCount = 0;
            if (showExpensive.HasValue && showExpensive.Value)
            {
                var expensiveData = await _service.GetExpensiveProductAsync(token);
                products = expensiveData.Select(e => new ProductListingDto
                {
                    ProductId = e.ProductId,
                    ProductName = e.ProductName,
                    UnitPrice = e.UnitPrice,
                    CategoryName = e.CategoryName ?? "Premium",
                    IsInStock = e.UnitsInStock > 0
                }).ToList();

                totalCount = products.Count();
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var searchResult = await _service.GetProductByName(name, token);
                products = searchResult.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                totalCount = searchResult.Count();
            }
            else
            {
                var pagedData = await _service.GetPagedDataAsync(pageNumber, pageSize, token);
                products = pagedData.Data;
                totalCount = pagedData.TotalCount;
            }
            ViewBag.Name = name;
            ViewBag.ShowExpensive = showExpensive;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("jwt");
            var prod = await _service.GetProductData(id,token);
            return View(model: prod);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("jwt");
            ViewBag.Categories = await _service.GetCategoriesAsync(token);
            ViewBag.Suppliers = await _service.GetSuppliersAsync(token);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
             var token = HttpContext.Session.GetString("jwt");
            var success = await _service.CreateProductAsync(dto,token);
            if (success)
            {
                TempData["Success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Failed to create product. Check API logs.");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("jwt");
            var productDetails = await _service.GetProductData(id,token);
            if (productDetails == null) return NotFound();
            ViewBag.Categories = await _service.GetCategoriesAsync(token);
            ViewBag.Suppliers = await _service.GetSuppliersAsync(token);
            var editDto = new ProductDto
            {
                ProductName = productDetails.ProductName,
                UnitPrice = productDetails.UnitPrice,
                CategoryId = productDetails.CategoryId,
                SupplierId = productDetails.SupplierId,
                QuantityPerUnit = productDetails.QuantityPerUnit,
                UnitsInStock = productDetails.UnitsInStock
            };

            return View(editDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto dto)
        {
            var token = HttpContext.Session.GetString("jwt");
            if (!ModelState.IsValid) return View(dto);
            var success = await _service.UpdateProductAsync(id, dto,token);
            if (success)
            {
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Update failed.");
            return View(dto);
        }

    }
}
