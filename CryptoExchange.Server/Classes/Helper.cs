using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Classes
{
    public class Helper
    {
        public static readonly Dictionary<Ticker, string> TickerDisplay = new Dictionary<Ticker, string>() {
            {Ticker.btceur, "Bitcoin (EUR)" },
            {Ticker.btcusd, "Bitcoin (USD)" }
        };
    }
}
