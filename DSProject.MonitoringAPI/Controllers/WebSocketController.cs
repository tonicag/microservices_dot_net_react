using DSProject.MonitoringAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace DSProject.MonitoringAPI.Controllers
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly SocketsManager _socketsManager;
        public WebSocketController(SocketsManager socketsManager)
        {
            _socketsManager = socketsManager;
        }
        [Route("/ws")]
        //[Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

                var userId = Guid.Parse(HttpContext.User.Claims.ToList()[1].Value);
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                _socketsManager.AddSocket(userId, webSocket);

                await Echo(webSocket);

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the server", CancellationToken.None);
                _socketsManager.RemoveSocket(userId);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private static async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

        }
    }
}
