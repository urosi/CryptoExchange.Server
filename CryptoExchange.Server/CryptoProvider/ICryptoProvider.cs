using CryptoExchange.Server.Classes;
using CryptoExchange.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.CryptoProvider
{
    public interface ICryptoProvider
    {
        public Task<OrderBook> GetOrderBook(Ticker ticker, float priceRangeLimitPercantage);
    }
}
