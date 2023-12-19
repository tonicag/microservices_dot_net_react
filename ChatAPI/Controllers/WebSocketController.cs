using DSProject.ChatAPI.Models;
using DSProject.ChatAPI.Service;
using DSProject.ChatAPI.Services;
using DSProject.ChatAPI.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace DSProject.ChatAPI.Controllers
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly SocketsManager _socketsManager;
        private readonly IChatService _chatService;
        public WebSocketController(SocketsManager socketsManager, IChatService chatService)
        {
            _socketsManager = socketsManager;
            _chatService = chatService;
        }
        [Route("/ws")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

                var userId = Guid.Parse(HttpContext.User.Claims.ToList()[1].Value);
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                _socketsManager.AddSocket(userId, webSocket);

                await Echo(webSocket, userId);

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the server", CancellationToken.None);
                _socketsManager.RemoveSocket(userId);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task Echo(WebSocket webSocket, Guid userId)
        {
            var buffer = new byte[2048 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                var msg = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                try
                {

                    var msg_request = JsonConvert.DeserializeObject<ChatRequestDTO>(msg);
                    if (msg_request.Sender_Id.ToString().Equals(userId.ToString()) && msg_request.Sender_Id != msg_request.Receiver_Id)
                    {
                        await _chatService.HandleChatRequest(msg_request);
                    }
                }
                catch (Exception ex) { }
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

            }

        }
    }
}
