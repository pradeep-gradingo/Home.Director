using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Home.Director.Api
{
    public class Program
    {
        private static IConfigurationRefresher _refresher = null;
        private static Timer _timer;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();
                    config.AddAzureAppConfiguration(options=>
                    {
                        options.Connect(settings["Connection:AzConnectionString"])
                        .ConfigureRefresh(refresh =>
                        {
                            refresh.Register("Settings:ArchivePath", "Home")
                            .Register("Settings:Enabled", "Home")
                            .Register("Settings:FlipHorizontal", "Home")
                            .Register("Settings:FlipVertical", "Home")
                            .Register("Settings:Frequency", "Home")
                            .Register("Settings:RawPath", "Home")
                            .Register("Settings:ReadyToUploadPath", "Home")
                            .SetCacheExpiration(TimeSpan.FromSeconds(120));
                        });

                        _refresher = options.GetRefresher();

                        _timer = new Timer(async (o) =>
                        {
                            await Program._refresher.RefreshAsync();
                        }, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(2));
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:786");
                });
    }
}
