using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.CryptoProvider
{
    public class OrderBookBitstamp
    {
        public long Microtimestamp { get; set; }
        public float[][] Bids { get; set; }
        public float[][] Asks { get; set; }
    }
}
