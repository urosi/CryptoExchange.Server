using CryptoExchange.Server.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Configuraitons
{
    public class BackgroundServiceConfig
    {
        public const string SECTION = "BackgroundServiceConfig";

        public int IntervalInMs { get; set; } 
        public float PriceRangeLimitPercantage { get; set; }
        public Ticker Ticker { get; set; }
    }
}
