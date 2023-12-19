using System.ComponentModel.DataAnnotations;

namespace DSProject.ChatAPI.Models
{

    public class ChatMesssage
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Sender_Id { get; set; }
        public Guid Receiver_Id { get; set; }
        public string Message_Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
