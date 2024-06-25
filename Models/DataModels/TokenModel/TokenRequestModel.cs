using System.ComponentModel.DataAnnotations;

namespace QuokkaMesh.Models.DataModel.TokenDataModel
{
    public class TokenRequestModel
    {
        [Required]

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
