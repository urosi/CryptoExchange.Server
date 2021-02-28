namespace CryptoExchange.Server.Model
{
    public class PriceQuote
    {
        public decimal Price { get; set; } 
        public decimal Volume { get; set; }
        public decimal SumVolume { get; set; }
    }
}
