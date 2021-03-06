﻿using CryptoExchange.Server.Classes;
using CryptoExchange.Server.Configuraiton;
using CryptoExchange.Server.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CryptoExchange.Server.CryptoProvider
{
    public class CryptoProviderBitstamp : ICryptoProvider
    {
        BitstampConfig _config;
        Uri _baseAddress;

        const int priceIndex = 0;
        const int volumeIndex = 1;

        public CryptoProviderBitstamp(IOptions<BitstampConfig> config)
        {
            _config = config.Value;
            _baseAddress = new Uri(_config.BaseUrl);
        }

        public async Task<OrderBook> GetOrderBook(Ticker ticker, float priceRangeLimitPercantage)
        {
            OrderBookBitstamp orderBookBitstamp = null;

            using (var client = new HttpClient()) {
                client.BaseAddress = _baseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"{_config.OrderBookUrl}{ticker}");

                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    orderBookBitstamp = JsonConvert.DeserializeObject<OrderBookBitstamp>(content);
                    Console.WriteLine($"CrytoProviderBitstamp.GetOrderBook({ticker}, {priceRangeLimitPercantage}): orderBbook with microTimestamp {orderBookBitstamp?.Microtimestamp} fetched successfully.");
                } else {
                    Console.WriteLine($"CryptoProviderBitstamp.GetOrderBook({ticker}, {priceRangeLimitPercantage}): Internal server Error.");
                }
            }

            return GetOrderBook(orderBookBitstamp, priceRangeLimitPercantage);
        }

        private OrderBook GetOrderBook(OrderBookBitstamp orderBookBitstamp, float priceRangeLimitPercantage)
        {
            List<PriceQuoteBid> bids = new List<PriceQuoteBid>();
            List<PriceQuoteAsk> asks = new List<PriceQuoteAsk>();

            decimal bidsSum = 0;
            decimal asksSum = 0;

            decimal limitFactor = Convert.ToDecimal(priceRangeLimitPercantage / 100.0);

            decimal askPriceLimit = orderBookBitstamp.Asks[0][priceIndex] * (1 + limitFactor);
            decimal bidPriceLimit = orderBookBitstamp.Bids[0][priceIndex] * (1 - limitFactor);

            foreach (var sourceAsk in orderBookBitstamp.Asks) {
                if (sourceAsk[priceIndex] > askPriceLimit) {
                    break;
                }
                asks.Add(GetPriceQuote<PriceQuoteAsk>(sourceAsk, ref asksSum));
            }

            foreach (var sourceBid in orderBookBitstamp.Bids) {
                if (sourceBid[priceIndex] < bidPriceLimit) {
                    break;
                }
                bidsSum += sourceBid[volumeIndex];
                bids.Add(GetPriceQuote<PriceQuoteBid>(sourceBid, ref bidsSum));
            }

            return new OrderBook {
                Timestamp = orderBookBitstamp.Microtimestamp,
                Asks = asks.ToArray(),
                Bids = bids.ToArray(),
            };

        }

        private T GetPriceQuote<T>(decimal[] priceQuote, ref decimal volumeSum) where  T: PriceQuote, new()
        {
            volumeSum += priceQuote[volumeIndex];
            return new T {
                Price = priceQuote[priceIndex],
                Volume = priceQuote[volumeIndex],
                SumVolume = volumeSum
            };
        }
    }
}
