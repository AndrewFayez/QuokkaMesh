using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels.CartCategory.Cart;

namespace QuokkaMesh.Models.DataModels.CartCategory.Category
{
    public class CategoryCart
    {
        public int CartId { get; set; }
        public virtual CartModel ? Cart { get; set; }

        public int CategoryId { get; set; }
        public virtual CategoryModel? Category { get; set; }
    }
}
