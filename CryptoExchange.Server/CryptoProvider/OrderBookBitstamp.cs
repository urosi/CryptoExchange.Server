using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.CryptoProvider
{
    public class OrderBookBitstamp
    {
        public long Microtimestamp { get; set; }
        public decimal[][] Bids { get; set; }
        public decimal[][] Asks { get; set; }
    }
}
