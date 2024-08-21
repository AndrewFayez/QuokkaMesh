using System.ComponentModel.DataAnnotations;

namespace QuokkaMesh.Models.DataModels.News
{
    public class NewsModel
    {

        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }

     //   public virtual ICollection<UserNews> UserNews { get; set; }

    }
}
