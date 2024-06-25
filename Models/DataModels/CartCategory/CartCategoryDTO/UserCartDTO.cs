namespace QuokkaMesh.Models.DataModels.CartCategory.CartCategoryDTO
{
    public class UserCartDTO
    {
        public string? Titel { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Content { get; set; }
        public bool? IsPremium { get; set; }
        public DateTime? Created { get; set; }
    }
}
