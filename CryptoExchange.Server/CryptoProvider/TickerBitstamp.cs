using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.CryptoProvider
{
    public class TickerBitstamp
    {
        public decimal Last { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Vwap { get; set; }
        public decimal Volume { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public long Timestamp { get; set; }
        public decimal Open { get; set; }

    }
}
