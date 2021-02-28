using CryptoExchange.Server.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Model
{
    public class OrderBook
    {
        public long Timestamp { get; set; }
        public Ticker Ticker { get; set; }
        public PriceQuote[] Bids { get; set; }
        public PriceQuote[] Asks { get; set; }
    }
}
