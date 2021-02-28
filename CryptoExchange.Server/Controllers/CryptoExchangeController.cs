using CryptoExchange.Server.CryptoProvider;
using CryptoExchange.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Controllers
{
    [Route("api/orderbook")]
    [ApiController]
    public class CryptoExchangeController : ControllerBase
    {
        //[HttpGet]
        //public async Task<ActionResult<OrderBook>> Get()
        //{
        //    var instance = TimerSingleton.Instance;
        //    instance.Timer.Change(Timeout.Infinite, Timeout.Infinite);
        //    return Ok();
            
        //    //return await new CryptoProviderBitstamp().GetOrderBook(Classes.Ticker.btceur);
        //}
    }
}
