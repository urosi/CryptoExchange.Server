using CryptoExchange.Server.BackgroundServices;
using CryptoExchange.Server.Configuraiton;
using CryptoExchange.Server.Configuraitons;
using CryptoExchange.Server.CryptoProvider;
using CryptoExchange.Server.Data;
using CryptoExchange.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoExchange.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BitstampConfig>(Configuration.GetSection(BitstampConfig.SECTION));
            services.Configure<BackgroundServiceConfig>(Configuration.GetSection(BackgroundServiceConfig.SECTION));

            services.AddDbContext<CryptoExchangeContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddControllers();
            services.AddSignalR();
            
            services.AddSingleton<CryptoProviderBackgroundService>();

            services.AddSingleton<ICryptoProvider, CryptoProviderBitstamp>();

            services.AddHostedService<BackgroundServiceStarter<CryptoProviderBackgroundService>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<OrderBookHub>("/orderbookhub");
            });
        }
    }
}
