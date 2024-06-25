using QuokkaMesh.Models.DataModels.CartCategory.Category;
using System.ComponentModel.DataAnnotations;

namespace QuokkaMesh.Models.DataModels.CartCategory.Cart
{
    public class UserCart
    {
        [Key]
        public int Id { get; set; }
        public string? Titel { get; set; }
        public string? ImageDesign { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Content { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public bool? IsPremium { get; set; }
        public bool? IsActive { get; set; } = true;
        public DateTime? Created { get; set; }
        public virtual ICollection<CartAndUserCart>? CartAndUserCart { get; set; }

    }
}
