using CryptoExchange.Server.Classes;
using CryptoExchange.Server.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CryptoExchange.Server.CryptoProvider
{
    public class CryptoProviderBitstamp : ICryptoProvider
    {
        const int priceIndex = 0;
        const int volumeIndex = 1;

        Uri _baseAddress = new Uri("https://www.bitstamp.net/");

        public async Task<OrderBook> GetOrderBook(Ticker ticker, int priceRange)
        {
            OrderBookBitstamp orderBookBitstamp = null;

            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("https://www.bitstamp.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method  
                HttpResponseMessage response = await client.GetAsync("api/v2/order_book/btceur");

                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    orderBookBitstamp = JsonConvert.DeserializeObject<OrderBookBitstamp>(content);
                    Console.WriteLine($"CrytoProviderBitstamp.GetOrderBook: orderBbook with microtimestamp {orderBookBitstamp?.Microtimestamp} fetched successfully.");
                } else {
                    Console.WriteLine("CryptoProviderBitstamp.GetOrderBook: Internal server Error.");
                }
            }

            return GetOrderBook(orderBookBitstamp, priceRange);
        }

        private OrderBook GetOrderBook(OrderBookBitstamp orderBookBitstamp, int priceRange)
        {
            List<PriceQuote> bids = new List<PriceQuote>();
            List<PriceQuote> asks = new List<PriceQuote>();

            float bidsSum = 0;
            float asksSum = 0;

            float askPriceLimit = orderBookBitstamp.Asks[0][priceIndex] + priceRange;
            float bidPriceLimit = orderBookBitstamp.Bids[0][priceIndex] - priceRange;


            foreach (var sourceAsk in orderBookBitstamp.Asks) {
                if (sourceAsk[priceIndex] > askPriceLimit) {
                    Console.WriteLine("Ask price limit reached. limit:" + askPriceLimit + ", askPrice:" +sourceAsk[priceIndex] + ", lowest ask:" + orderBookBitstamp.Asks[0][priceIndex]);
                    break;
                }
                asksSum += sourceAsk[volumeIndex];
                asks.Add(new PriceQuote {
                    Price = sourceAsk[priceIndex],
                    Volume = sourceAsk[volumeIndex],
                    SumVolume = asksSum,
                });
            }

            foreach (var sourceBid in orderBookBitstamp.Bids) {
                if (sourceBid[priceIndex] < bidPriceLimit) {
                    Console.WriteLine("bid price limit reached. limit:" + bidPriceLimit + ", bidPrice:" + sourceBid[priceIndex] + ", highest bid:" + orderBookBitstamp.Bids[0][priceIndex]);
                    break;
                }
                bidsSum += sourceBid[volumeIndex];
                bids.Add(new PriceQuote {
                    Price = sourceBid[priceIndex],
                    Volume = sourceBid[volumeIndex],
                    SumVolume = bidsSum,
                });
            }




            //for (int i = 0; i < priceRange; i++) {                
            //    bids.Add(GetPriceQuote(orderBookBitstamp.Bids[i], ref bidsSum));
            //    asks.Add(GetPriceQuote(orderBookBitstamp.Asks[i], ref asksSum));
            //}


            return new OrderBook {
                Timestamp = orderBookBitstamp.Microtimestamp,
                Asks = asks.ToArray(),
                Bids = bids.ToArray(),
            };

        }

        private PriceQuote GetPriceQuote(float[] priceQuote, ref float volumeSum)
        {
            volumeSum += priceQuote[volumeIndex];
            return new PriceQuote {
                Price = priceQuote[priceIndex],
                Volume = priceQuote[volumeIndex],
                SumVolume = volumeSum
            };
        }
    }
}
