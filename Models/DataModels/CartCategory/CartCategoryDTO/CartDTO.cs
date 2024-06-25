using System.ComponentModel;

namespace QuokkaMesh.Models.DataModels.CartCategory.CartCategoryDTO
{
    public class CartDTO
    {
        public string? Titel { get; set; }
        public IFormFile? ImageDesign { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Content { get; set; }
        public DateTime? Created { get; set; }
        public int ? NumberOfPoint { get; set; }
        public bool? IsPremium { get; set; } = false;

    }
}
