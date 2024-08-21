namespace QuokkaMesh.Models.DataModels
{
    public class SendMessageRequestModel
    {
        public int Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
    }
}
