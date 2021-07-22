using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services
{
    public class DatabaseSyncService : IHostedService, IDisposable
    {
        private readonly ILogger<DatabaseSyncService> _logger;
        private IHttpClientFactory _httpClientFactory;
        private IDbContextFactory<CountryHolidaysContext> _context;
        private readonly DatabaseSyncServiceOptions _options;

        private Timer _timer;
        private HttpClient _client;
        private CancellationTokenSource _tokenSource;

        public DatabaseSyncService(ILogger<DatabaseSyncService> logger, 
            IHttpClientFactory httpClientFactory, 
            IDbContextFactory<CountryHolidaysContext> countryHolidayContextFactory,
            IOptions<DatabaseSyncServiceOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _context = countryHolidayContextFactory;
            _options = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database Synchronization Service running.");

            _client = _httpClientFactory.CreateClient();

            _timer = new Timer(UpdateDatabaseFromExternal, null, TimeSpan.Zero, TimeSpan.FromHours(_options.SyncDelayHours));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database Synchronization Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _tokenSource?.Cancel();
            return Task.CompletedTask;
        }

        private async void UpdateDatabaseFromExternal(object state)
        {
            _tokenSource = new CancellationTokenSource();
            var cancellationToken = _tokenSource.Token;

            var context = _context.CreateDbContext();

            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _client.GetAsync(EnricoAPI.GetSupportedCountriesRequest(), cancellationToken);
                
                var countriesList = await EnricoAPI.ParseCountries(await result.Content.ReadAsStringAsync(), cancellationToken);

                await EnricoAPI.SyncCountries(context, countriesList, cancellationToken);

                _logger.LogInformation("Countries synced.");

                await context.Countries.ForEachAsync(async c =>
                {
                    if (c.Regions.Count != 0)
                    {
                        foreach (var region in c.Regions)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            
                            _logger.LogInformation("Waiting before request");
                            await Task.Delay(TimeSpan.FromHours(1)/3000);
                            var result = await _client.GetAsync(EnricoAPI.GetHolidaysRequest(c.SupportedFromDate, DateTime.Now.AddYears(1), c.CountryCode, region.RegionCode), cancellationToken);

                            var holidaysList = await EnricoAPI.ParseHolidays(await result.Content.ReadAsStringAsync(), c, region, cancellationToken);

                            await EnricoAPI.SyncHolidays(context, holidaysList, cancellationToken);
                        }
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var result = await _client.GetAsync(EnricoAPI.GetHolidaysRequest(c.SupportedFromDate, c.SupportedToDate, c.CountryCode, null), cancellationToken);
                        var holidaysList = EnricoAPI.ParseHolidays(await result.Content.ReadAsStringAsync(), c, null, cancellationToken);

                        await EnricoAPI.SyncHolidays(context, holidaysList.Result, cancellationToken);
                    }
                });

                _logger.LogInformation("Holidays synced.");
                await context.Database.CommitTransactionAsync();
                _logger.LogInformation("Database synced.");
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Failed syncing database. Reason: {ex.Message}");
            }

            await context.DisposeAsync();
            _tokenSource?.Dispose();
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _client?.Dispose();
            _tokenSource?.Dispose();
        }
    }
}
