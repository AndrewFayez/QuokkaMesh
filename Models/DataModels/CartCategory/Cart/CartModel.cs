using QuokkaMesh.Models.DataModels.CartCategory.Category;

namespace QuokkaMesh.Models.DataModels.CartCategory.Cart
{
    public class CartModel
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public string? ImageDesign { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Content { get; set; }
        public int ? NumberOfPoint { get; set; }
        public bool ? IsPremium { get; set; } = false;
        public bool? IsActive { get; set; } = true; 
        public DateTime? Created { get; set; }
        public virtual ICollection<CategoryCart>? CategoryCart { get; set; }
        public virtual ICollection<CartAndUserCart>? CartAndUserCart { get; set; }


    }
}
