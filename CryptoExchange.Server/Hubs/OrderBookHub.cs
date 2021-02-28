using CryptoExchange.Server.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Hubs
{
    public class OrderBookHub : Hub<IOrderBookClient>
    {
        public readonly static Connections Connections = new Connections();

        public override Task OnConnectedAsync()
        {
            Connections.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
