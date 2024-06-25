namespace QuokkaMesh.Models.DataModels.CartCategory.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public string? Image { get; set; }
        public bool? IsActive { get; set; } = true;

        public virtual ICollection<CategoryCart>? CategoryCart { get; set; }

    }
}
