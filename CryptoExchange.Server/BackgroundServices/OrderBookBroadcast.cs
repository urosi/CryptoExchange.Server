using CryptoExchange.Server.Classes;
using CryptoExchange.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.BackgroundServices
{
    public class OrderBookBroadcast
    {
        public float Timestamp { get; private set; }
        public string TickerDisplay { get; set; }
        public decimal[][] Bids { get; private set; }
        public decimal[][] Asks { get; private set; }

        public OrderBookBroadcast(OrderBook orderBook)
        {
            List<decimal[]> asks = new List<decimal[]>();
            List<decimal[]> bids = new List<decimal[]>();

            foreach(var sourceAsks in orderBook.Asks) {
                asks.Add(new decimal[] { sourceAsks.Price, sourceAsks.SumVolume });
            }

            foreach(var sourceBids in orderBook.Bids) {
                bids.Add(new decimal[] { sourceBids.Price, sourceBids.SumVolume });
            }

            Timestamp = orderBook.Timestamp;
            TickerDisplay = Helper.TickerDisplay[orderBook.Ticker];
            Asks = asks.ToArray();
            Bids = bids.ToArray();
        }
    }
}
