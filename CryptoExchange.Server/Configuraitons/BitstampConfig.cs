using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Configuraiton
{
    public class BitstampConfig
    {
        public const string SECTION = "BitstampConfig";

        public string BaseUrl { get; set; }
        public string OrderBookUrl { get; set; }
    }
}
