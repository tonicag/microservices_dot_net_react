using DSProject.ChatAPI.Models;

namespace DSProject.ChatAPI.Services.IService
{
    public interface IChatService
    {
        public Task<bool> HandleChatRequest(ChatRequestDTO chatRequestDTO);
    }
}
