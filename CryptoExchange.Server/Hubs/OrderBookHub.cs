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
    }
}
