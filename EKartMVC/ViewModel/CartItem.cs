namespace EKartMVC.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public short UnitsInStock { get; set; }

        public CartItem() { }

        public CartItem(int productId, string productName, decimal unitPrice, short quantity, short unitsInStock)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            UnitsInStock = unitsInStock;
        }

        public decimal GetTotal()
        {
            return UnitPrice * Quantity;
        }
    }
}
