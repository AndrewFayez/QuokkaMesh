using QuokkaMesh.Models.DataModels.CartCategory.Category;

namespace QuokkaMesh.Models.DataModels.CartCategory.Cart
{
    public class CartAndUserCart
    {
        public int CartId { get; set; }
        public virtual CartModel? Cart { get; set; }

        public int UserCartId { get; set; }
        public virtual UserCart? UserCart { get; set; }
    }
}
