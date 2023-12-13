using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace DSProject.MonitoringAPI.Service
{
    public class SocketsManager
    {
        private ConcurrentDictionary<Guid, WebSocket> _sockets = new ConcurrentDictionary<Guid, WebSocket>();

        public void AddSocket(Guid userId, WebSocket socket)
        {
            _sockets.TryAdd(userId, socket);
        }

        public void RemoveSocket(Guid userId)
        {
            _sockets.TryRemove(userId, out _);
        }

        public async Task SendAsync(Guid? userId, object obj)
        {

            if (_sockets.TryGetValue((Guid)userId, out var socket) && socket.State == WebSocketState.Open)
            {
                var msg = JsonConvert.SerializeObject(obj);
                await socket.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
