namespace DSProject.ChatAPI.Models
{
    public class ChatRequestDTO
    {

        public Guid Sender_Id { get; set; }
        public Guid Receiver_Id { get; set; }
        public string Message_Content { get; set; }
        public string Type { get; set; }
    }
}
