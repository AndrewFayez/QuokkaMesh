
using Microsoft.AspNetCore.Identity;
using QuokkaMesh.Models.DataModel.TokenDataModel;
using QuokkaMesh.Models.DataModels;
using QuokkaMesh.Models.DataModels.CartCategory.Cart;
using QuokkaMesh.Models.DataModels.News;

namespace QuokkaMesh.Models.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string? FullName { get; set; }
   
        public string? Email { get; set; }
        public string? ImageCover { get; set; }
        public DateTime? DateTime { get; set; }
        public int? CountPoint { get; set; } = 0;
        public bool? IsPremium { get; set; } = false;
        public bool? IsAdmin { get; set; } = false;

        public List<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<UserCart>? UserCart { get; set; }
        public virtual ICollection<UserMessage> UserMessage { get; set; }

        //public virtual ICollection<UserNews> UserNews { get; set; }


    }

}
