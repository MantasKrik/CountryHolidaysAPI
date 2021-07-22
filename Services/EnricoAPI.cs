using CountryHolidaysAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CountryHolidaysAPI.Services
{
    public static class EnricoAPI
    {
        public static string GetSupportedCountriesRequest()
        {
            return "https://kayaposoft.com/enrico/json/v2.0/?action=getSupportedCountries";
        }

        public static string GetHolidaysRequest(DateTime? fromDate, DateTime? toDate, string countryCode, string region)
        {
            string request = $"https://kayaposoft.com/enrico/json/v2.0?action=getHolidaysForDateRange&";

            if (fromDate.HasValue)
                request += $"fromDate={fromDate.Value.Day}-{fromDate.Value.Month}-{fromDate.Value.Year}&";

            if (toDate.HasValue)
                request += $"toDate={toDate.Value.Day}-{toDate.Value.Month}-{toDate.Value.Year}&";

            if (!string.IsNullOrEmpty(countryCode))
            {
                request += $"country={countryCode}&";

                if (!string.IsNullOrEmpty(region))
                {
                    request += $"region={region}&";
                }
            }

            return request;
        }

        public static Task<List<Holiday>> ParseHolidays(string json, Country country, Region region, CancellationToken? cancellationToken = null)
        {          
            var holidays = new List<Holiday>();

            var result = JsonSerializer.Deserialize<List<EnricoHoliday>>(json);

            foreach (var enricoHoliday in result)
            {
                if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                    cancellationToken.Value.ThrowIfCancellationRequested();

                holidays.Add(ParseHoliday(enricoHoliday, country, region));
            }

            return Task.FromResult(holidays);
        }

        private static Holiday ParseHoliday(EnricoHoliday enricoHoliday, Country country, Region region)
        {
            Holiday holiday = new Holiday();
            holiday.Country = country;
            holiday.CountryId = country.Id;

            foreach(var name in enricoHoliday.name)
            {
                holiday.HolidayNames.Add(new HolidayName() { Holiday = holiday, Language = name.lang, Text = name.text });
            }

            holiday.HolidayType = ParseHolidayType(enricoHoliday.holidayType);
            holiday.Date = new DateTime(enricoHoliday.date.year <= 9999 ? enricoHoliday.date.year : 9999, enricoHoliday.date.month, enricoHoliday.date.day);
            holiday.Region = region;

            return holiday;
        }

        private static HolidayType ParseHolidayType(string holidayType)
        {
            return holidayType switch
            {
                "public_holiday" => HolidayType.PublicHoliday,
                "observance" => HolidayType.Observance,
                "school_holiday" => HolidayType.SchoolHoliday,
                "other_day" => HolidayType.OtherDay,
                "extra_working_day" => HolidayType.ExtraWorkingDay,
                _ => HolidayType.OtherDay,
            };
        }

        public static Task<List<Country>> ParseCountries(string json, CancellationToken? cancellationToken)
        {
            var countries = new List<Country>();
            
            var result = JsonSerializer.Deserialize<List<EnricoCountry>>(json);

            foreach(var enricoCountry in result)
            {
                if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                    cancellationToken.Value.ThrowIfCancellationRequested();

                countries.Add(ParseCountry(enricoCountry));
            }

            return Task.FromResult(countries);
        }

        private static Country ParseCountry(EnricoCountry enricoCountries)
        {
            Country country = new Country();

            country.CountryCode = enricoCountries.countryCode;
            country.FullName = enricoCountries.fullName;
            country.Regions = ParseRegions(enricoCountries.regions, country);
            country.SupportedFromDate = new DateTime(enricoCountries.fromDate.year <= 9999 ? enricoCountries.fromDate.year : 9999, enricoCountries.fromDate.month, enricoCountries.fromDate.day);
            country.SupportedToDate = new DateTime(enricoCountries.toDate.year <= 9999 ? enricoCountries.toDate.year : 9999, enricoCountries.toDate.month, enricoCountries.toDate.day);

            return country;
        }

        private static List<Region> ParseRegions(List<string> regions, Country country)
        {
            var countryRegions = new List<Region>();

            foreach(var region in regions)
            {
                countryRegions.Add(new Region() { Country = country, RegionCode = region });
            }

            return countryRegions;
        }

        public static async Task SyncCountries(CountryHolidaysContext context, List<Country> countries, CancellationToken cancellationToken)
        {
            foreach (var country in countries)
            {
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                var existingCountry = await context.Countries.FirstOrDefaultAsync(c => c.CountryCode == country.CountryCode);

                if (existingCountry != null)
                {
                    existingCountry.SupportedFromDate = country.SupportedFromDate;
                    existingCountry.SupportedToDate = country.SupportedToDate;
                    context.Entry<Country>(existingCountry).State = EntityState.Modified;
                }
                else
                {
                    await context.Countries.AddAsync(country);
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SyncHolidays(CountryHolidaysContext context, List<Holiday> holidays, CancellationToken cancellationToken)
        {
            foreach (var holiday in holidays)
            {
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                var existingHoliday = await context.Holidays.FirstOrDefaultAsync(h => h.CountryId == holiday.CountryId);

                if (existingHoliday != null)
                {
                    // Update holiday
                }
                else
                {
                    await context.Holidays.AddAsync(holiday);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
