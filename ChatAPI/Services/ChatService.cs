using DSProject.ChatAPI.Data;
using DSProject.ChatAPI.Models;
using DSProject.ChatAPI.Service;
using DSProject.ChatAPI.Services.IService;
using RabbitMQ.Client;

namespace DSProject.ChatAPI.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _db;
        private readonly SocketsManager _socketsManager;
        private readonly ILogger _logger;
        public ChatService(AppDbContext dbContext, SocketsManager manager, ILogger<ChatService> logger)
        {
            _db = dbContext;
            _socketsManager = manager;
            _logger = logger;
        }
        public async Task<bool> HandleChatRequest(ChatRequestDTO chatRequestDTO)
        {
            if (chatRequestDTO == null) return false;
            switch (chatRequestDTO.Type)
            {
                case RequestTypes.CHAT_INIT:
                    _logger.Log(LogLevel.Information, "CHAT_INIT");
                    chatRequestDTO.Type = RequestTypes.CHAT_INIT;
                    await _socketsManager.SendAsync(chatRequestDTO.Receiver_Id, chatRequestDTO);
                    break;
                case RequestTypes.MSG_SEND:
                    chatRequestDTO.Type = RequestTypes.MSG_RECEIVE;
                    AddChatMessage(chatRequestDTO);
                    await _socketsManager.SendAsync(chatRequestDTO.Receiver_Id, chatRequestDTO);
                    break;
                case RequestTypes.TYIPING_SEND:
                    chatRequestDTO.Type = RequestTypes.TYIPING_RECEIVE;
                    await _socketsManager.SendAsync(chatRequestDTO.Receiver_Id, chatRequestDTO);
                    break;
                case RequestTypes.SEEN_SEND:
                    chatRequestDTO.Type = RequestTypes.SEEN_RECEIVE;
                    await _socketsManager.SendAsync(chatRequestDTO.Receiver_Id, chatRequestDTO);
                    break;

                default:
                    break;

            }
            return true;

        }
        private void AddChatMessage(ChatRequestDTO chatRequestDTO)
        {
            _db.Add(new ChatMesssage
            {
                Message_Content = chatRequestDTO.Message_Content,
                Receiver_Id = chatRequestDTO.Receiver_Id,
                Sender_Id = chatRequestDTO.Sender_Id,
                Timestamp = DateTime.UtcNow

            });
            _db.SaveChanges();
        }
    }
}
