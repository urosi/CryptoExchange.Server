using CryptoExchange.Server.BackgroundServices;
using CryptoExchange.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Hubs
{
    public interface IOrderBookClient
    {
        Task BroadcastOrderBook(OrderBookBroadcast orderBook);
    }
}
