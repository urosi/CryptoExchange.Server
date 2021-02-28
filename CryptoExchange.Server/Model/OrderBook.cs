using CryptoExchange.Server.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Model
{
    public class OrderBook
    {
        public long Id { get; set; }
        public long Timestamp { get; set; }
        public Ticker Ticker { get; set; }
        public virtual ICollection<PriceQuoteBid> Bids { get; set; }
        public virtual ICollection<PriceQuoteAsk> Asks { get; set; }
    }
}
