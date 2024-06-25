using System.Text.Json.Serialization;

namespace QuokkaMesh.Models.DataModels.UserModel
{
    public class AuthModel
    {
        public string Id { get; set; }

        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

       
    }

}
