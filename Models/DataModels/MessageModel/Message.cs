
namespace QuokkaMesh.Models.DataModels
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string ReciverId { get; set; }
       
        public virtual ICollection<UserMessage> UserMessage { get; set; }
    }
}
