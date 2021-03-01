using CryptoExchange.Server.Classes;
using CryptoExchange.Server.Configuraiton;
using CryptoExchange.Server.Configuraitons;
using CryptoExchange.Server.CryptoProvider;
using CryptoExchange.Server.Data;
using CryptoExchange.Server.Hubs;
using CryptoExchange.Server.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoExchange.Server.BackgroundServices
{
    public class CryptoProviderBackgroundService : BackgroundService
    {
        private readonly IHubContext<OrderBookHub, IOrderBookClient> _orderBookHub;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly CryptoExchangeContext _dbContext;
        private readonly BackgroundServiceConfig _config;

        public CryptoProviderBackgroundService(ICryptoProvider cryptoProvider, IHubContext<OrderBookHub, IOrderBookClient> orderBookHub, IServiceScopeFactory factory, IOptions<BackgroundServiceConfig> config)
        {
            _orderBookHub = orderBookHub;
            _cryptoProvider = cryptoProvider;
            _config = config.Value;

           _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<CryptoExchangeContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (OrderBookHub.Connections.Count > 0)
                {
                    try 
                    {
                        var orderBook = await _cryptoProvider.GetOrderBook(_config.Ticker, _config.PriceRangeLimitPercantage);
                        orderBook.Ticker = _config.Ticker;

                        if (_config.LogDb)
                        {
                            StoreOrderBook(orderBook);
                        }
                       
                        var orderBookBroadcast = new OrderBookBroadcast(orderBook);
                        await _orderBookHub.Clients.All.BroadcastOrderBook(orderBookBroadcast);
                    } 
                    catch (Exception ex) 
                    {
                        Console.WriteLine("Error in CryptoProviderBackgroundService.ExecuteAsync:" + ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("There are no clients connected.");
                }

                await Task.Delay(_config.IntervalInMs);
            }
        }

        private void StoreOrderBook(OrderBook orderBook)
        {
            _dbContext.OrderBook.Add(orderBook);
            _dbContext.SaveChanges();
        }
    }
}
