
using QuokkaMesh.Models.Data;

namespace QuokkaMesh.Models.DataModels
{
    public class UserMessage
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int MessageId { get; set; }
        public virtual Message Messages { get; set; }
    }
}
