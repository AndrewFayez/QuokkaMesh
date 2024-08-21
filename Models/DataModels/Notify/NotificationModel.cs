using System.ComponentModel.DataAnnotations;

namespace QuokkaMesh.Models.DataModels.Notify
{
    public class NotificationModel
    {
        [Key]
       public int Id { get; set; }
        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;  
    }
}
