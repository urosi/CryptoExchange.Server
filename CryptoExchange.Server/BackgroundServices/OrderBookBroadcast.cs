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
        public float[][] Bids { get; private set; }
        public float[][] Asks { get; private set; }

        public OrderBookBroadcast(OrderBook orderBook)
        {
            List<float[]> asks = new List<float[]>();
            List<float[]> bids = new List<float[]>();

            foreach(var sourceAsks in orderBook.Asks) {
                asks.Add(new float[] { sourceAsks.Price, sourceAsks.SumVolume });
            }

            foreach(var sourceBids in orderBook.Bids) {
                bids.Add(new float[] { sourceBids.Price, sourceBids.SumVolume });
            }

            Bids = bids.ToArray();
            Asks = asks.ToArray();
            Timestamp = orderBook.Timestamp;
        }
    }
}
