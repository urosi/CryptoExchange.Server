using CryptoExchange.Server.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Data
{
    public class CryptoExchangeContext : DbContext
    {
        public CryptoExchangeContext(DbContextOptions<CryptoExchangeContext> options) : base(options)
        {

        }

        public DbSet<OrderBook> OrderBook {get; set;}
        
    }
}
