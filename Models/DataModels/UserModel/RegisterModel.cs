namespace QuokkaMesh.Models.DataModels.UserModel
{
    public class RegisterModel
    {
        public string? FullName { get; set; }
        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Phonenumber { get; set; }



        public IFormFile? ImageCover { get; set; }

       // public DateTime? DateTime { get; set; }


    }
}
