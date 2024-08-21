namespace QuokkaMesh.Models.DataModels
{
    public class SendMessageRequest
    {

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
    }
}
