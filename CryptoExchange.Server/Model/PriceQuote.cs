using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoExchange.Server.Model
{
    public class PriceQuote
    {
        public long Id { get; set; }

        [Column(TypeName = "decimal(20,10)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(20,10)")]
        public decimal Volume { get; set; }

        [Column(TypeName = "decimal(20,10)")]
        public decimal SumVolume { get; set; }

        public virtual OrderBook OrderBook { get; set; }
    }

    public class PriceQuoteBid : PriceQuote
    {

    }

    public class PriceQuoteAsk : PriceQuote
    {

    }
}
