using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Home.Director.Api.Workers
{
    public class ImageCollector : BackgroundService
    {
        private readonly ILogger<ImageCollector> _logger;
        private readonly HttpClient _httpClient;
        public ImageCollector(ILogger<ImageCollector> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.100:786/");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting image collector.");
            while (!stoppingToken.IsCancellationRequested)
            {
                Settings settings = null;
                try
                {
                    var configJson = await _httpClient.GetStringAsync("config");
                    settings = JsonSerializer.Deserialize<Settings>(configJson);
                    _logger.LogInformation($"Image Harvesting IsEnabled: {settings.Enabled}");
                    if (settings.Enabled)
                    {
                        _logger.LogInformation("Harvesting Image...");
                        var imageData = await _httpClient.GetByteArrayAsync("/camera/images");
                        var path = Path.Combine(settings.RawPath, $"Capture_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg");
                        var fs = File.Create(path);
                        fs.Write(imageData, 0, imageData.Length);
                        await fs.FlushAsync();
                        fs.Close();
                        _logger.LogInformation($"Harvested Image: {path}");
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong");
                }
                Thread.Sleep(settings?.Frequency ?? 5000);
            }
            _logger.LogInformation("Stopping image collector.");
        }
    }
}
