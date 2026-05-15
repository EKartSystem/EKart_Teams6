using System.Text.Json;

namespace EKartMVC.Services
{
    public class CartService
    {
        private const string CartSessionKey = "ShoppingCart";
        private readonly ISession _session;

        public CartService(ISession session)
        {
            _session = session;
        }

        public List<Models.CartItem> GetCart()
        {
            var cartJson = _session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<Models.CartItem>();
            }
            return JsonSerializer.Deserialize<List<Models.CartItem>>(cartJson) ?? new List<Models.CartItem>();
        }

        public void AddToCart(Models.CartItem item)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveAll(c => c.ProductId == productId);
            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, short quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveFromCart(productId);
                }
                else
                {
                    item.Quantity = quantity;
                    SaveCart(cart);
                }
            }
        }

        public void ClearCart()
        {
            _session.Remove(CartSessionKey);
        }

        public decimal GetCartTotal()
        {
            return GetCart().Sum(c => c.GetTotal());
        }

        private void SaveCart(List<Models.CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            _session.SetString(CartSessionKey, cartJson);
        }
    }
}
