using EKartMVC.Models;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductApiService _productService;

        public CartController(ProductApiService productService)
        {
            _productService = productService;
        }

        private CartService GetCartService()
        {
            return new CartService(HttpContext.Session);
        }

        public IActionResult Index()
        {
            var cartService = GetCartService();
            var cart = cartService.GetCart();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, short quantity = 1)
        {
            var token = HttpContext.Session.GetString("jwt");
            var product = await _productService.GetProductData(productId, token);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = new CartItem(product.ProductId, product.ProductName, product.UnitPrice ?? 0, quantity, product.UnitsInStock ?? 0);
            var cartService = GetCartService();
            cartService.AddToCart(cartItem);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cartService = GetCartService();
            cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(int productId, short quantity)
        {
            var cartService = GetCartService();
            cartService.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            var cartService = GetCartService();
            cartService.ClearCart();
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cartService = GetCartService();
            var cart = cartService.GetCart();
            if (cart.Count == 0)
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Checkout", "Order");
        }
    }
}
