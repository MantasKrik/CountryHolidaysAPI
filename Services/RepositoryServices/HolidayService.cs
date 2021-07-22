using CountryHolidaysAPI.Models;
using CountryHolidaysAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services.RepositoryServices
{
    public class HolidayService : IHolidayService
    {
        private IHolidayRepositoryExtension _holidayRepository;
        private ICountryRepositoryExtension _countryRepository;
        private IHttpClientFactory _httpClientFactory;
        private ICountryService _countryService;
        private HttpClient _httpClient;

        public HolidayService(IHolidayRepositoryExtension holidayRepository,
            ICountryRepositoryExtension countryRepository,  
            IHttpClientFactory httpClientFactory,
            ICountryService countryService)
        {
            _holidayRepository = holidayRepository;
            _countryRepository = countryRepository;
            _httpClientFactory = httpClientFactory;

            _countryService = countryService;
            
            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task<object> GetDayStatus(string countryCode, int day, int month, int year)
        {
            if (await _countryRepository.IsEmpty())
            {
                await _countryService.SyncCountries();
            }

            if (await _holidayRepository.IsEmpty(countryCode, year))
            {
                if (!await SyncHolidays(countryCode, year))
                    return null;
            }

            return await _holidayRepository.GetDayStatus(countryCode, day, month, year);
        }

        public async Task<IEnumerable<object>> GetGroupedByMonthHolidays(string countryCode, int year)
        {
            if(await _countryRepository.IsEmpty())
            {
                await _countryService.SyncCountries();
            }

            var holidayRepositoryList = await _holidayRepository.GetGroupedByMonth(countryCode, year);

            if (holidayRepositoryList == null || holidayRepositoryList.Count() == 0)
            {
                if (await SyncHolidays(countryCode, year))
                {
                    return await _holidayRepository.GetGroupedByMonth(countryCode, year);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return holidayRepositoryList;
            }


        }

        public async Task<object> GetMaxFreeDays(string countryCode, int year)
        {
            if (await _countryRepository.IsEmpty())
            {
                await _countryService.SyncCountries();
            }

            if (await _holidayRepository.IsEmpty(countryCode, year))
            {
                if (!await SyncHolidays(countryCode, year))
                    return null;
            }

            return _holidayRepository.GetMaximumFreeDays(countryCode, year);
        }

        private async Task<bool> SyncHolidays(string countryCode, int year)
        {
            var country = await _countryRepository.Get(countryCode);

            if (country == null)
                return false;

            if (country.Regions.Count != 0)
            {
                foreach (var region in country.Regions)
                {
                    await Task.Delay(TimeSpan.FromHours(1) / 3000);

                    DateTime fromDate = new DateTime(year, 1, 1);
                    DateTime toDate = new DateTime(year, 12, 31);
                    var result = await _httpClient.GetAsync(EnricoAPI.GetHolidaysRequest(fromDate, toDate, country.CountryCode, region.RegionCode));

                    var holidaysList = await EnricoAPI.ParseHolidays(await result.Content.ReadAsStringAsync(), country, region);

                    await _holidayRepository.CreateRange(holidaysList);
                }
            }
            else
            {
                DateTime fromDate = new DateTime(year, 1, 1);
                DateTime toDate = new DateTime(year, 12, 31);
                var result = await _httpClient.GetAsync(EnricoAPI.GetHolidaysRequest(fromDate, toDate, country.CountryCode, null));
                var holidaysList = await EnricoAPI.ParseHolidays(await result.Content.ReadAsStringAsync(), country, null);

                await _holidayRepository.CreateRange(holidaysList);
            }

            return true;
        }
    }
}
