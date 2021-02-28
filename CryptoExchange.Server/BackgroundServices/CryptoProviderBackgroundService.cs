using CryptoExchange.Server.Classes;
using CryptoExchange.Server.CryptoProvider;
using CryptoExchange.Server.Data;
using CryptoExchange.Server.Hubs;
using CryptoExchange.Server.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoExchange.Server.BackgroundServices
{
    public class CryptoProviderBackgroundService : BackgroundService
    {
        private const int DelayInMs = 2000;
        private const float PriceRangeLimitPercantage = 1.2F;
        private const Ticker ticker = Ticker.btceur;

        private readonly IHubContext<OrderBookHub, IOrderBookClient> _orderBookHub;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly CryptoExchangeContext _dbContext;

        public CryptoProviderBackgroundService(ICryptoProvider cryptoProvider, IHubContext<OrderBookHub, IOrderBookClient> orderBookHub, IServiceScopeFactory factory)
        {
            _orderBookHub = orderBookHub;
            _cryptoProvider = cryptoProvider;
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
                        var orderBook = await _cryptoProvider.GetOrderBook(ticker, PriceRangeLimitPercantage);
                        _dbContext.OrderBook.Add(orderBook);
                        _dbContext.SaveChanges();
                        var orderBookBroadcast = new OrderBookBroadcast(orderBook);
                        await _orderBookHub.Clients.All.BroadcastOrderBook(orderBookBroadcast);
                    } 
                    catch (Exception ex) 
                    {
                        Console.WriteLine("Error in ExecuteAsync" + ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("There are no clients connected.");
                }

                await Task.Delay(DelayInMs);
            }
        }

    }
}
