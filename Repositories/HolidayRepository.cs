using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public class HolidayRepository : IHolidayRepositoryExtension
    {
        private readonly CountryHolidaysContext _context;

        public HolidayRepository(CountryHolidaysContext context)
        {
            _context = context;
        }

        public async Task<Holiday> Create(Holiday holiday)
        {
            _context.Add(holiday);
            await _context.SaveChangesAsync();

            return holiday;
        }

        public async Task CreateRange(List<Holiday> entries)
        {
            await _context.AddRangeAsync(entries);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var holidayToDelete = await _context.Holidays.FindAsync(id);
            _context.Holidays.Remove(holidayToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Holiday>> Get()
        {
            return await _context.Holidays.ToListAsync();
        }

        public async Task<Holiday> Get(int id)
        {
            return await _context.Holidays.FindAsync(id);
        }

        public async Task<IEnumerable<object>> GetDayStatus(string countryCode, int? day, int? month, int? year)
        {
            IQueryable<Holiday> query = _context.Holidays;

            if (!string.IsNullOrEmpty(countryCode))
                query = query.Include(h => h.Country).Where(h => h.Country.CountryCode.Equals(countryCode));

            if (year.HasValue)
                query = query.Where(h => h.Date.Year.Equals(year));

            if (month.HasValue)
                query = query.Where(h => h.Date.Month.Equals(month));

            if (day.HasValue)
                query = query.Where(h => h.Date.Day.Equals(day));

            var response = await query.Select(h => new { status = h.HolidayType.GetDisplayName() } ).ToListAsync();

            if(response != null && response.Count != 0)
            {
                return response;
            }
            else
            {
                var requestedDayOfWeek = new DateTime(year.Value, month.Value, day.Value).DayOfWeek;
                if (requestedDayOfWeek != DayOfWeek.Saturday && requestedDayOfWeek != DayOfWeek.Sunday)
                    return new List<string>() { "Workday" };
                else
                    return new List<string>() { "Weekend" };
            }
        }

        public async Task<IEnumerable<object>> GetGroupedByMonth(string countryCode, int? year)
        {
            IQueryable<Holiday> query = _context.Holidays.Include(h => h.HolidayNames);

            if (!string.IsNullOrEmpty(countryCode))
                query = query.Include(h => h.Country).Where(h => h.Country.CountryCode.Equals(countryCode));

            if (year.HasValue)
                query = query.Where(h => h.Date.Year.Equals(year));

            var filteredList = await query.ToListAsync();
                
            return filteredList.GroupBy(h => h.Date.Month, (x, y) => new { month = x, holidays = y.ToList()})
                .ToList();
        }

        public async Task<object> GetMaximumFreeDays(string countryCode, int year)
        {
            int maxFreeDays = int.MinValue;
            int currentFreeDays = 0;

            var query = _context.Holidays.AsNoTracking().Include(h => h.Country)
                .Where(h => h.Country.CountryCode.Equals(countryCode))
                .Where(h => h.Date.Year.Equals(year));

            if (await query.FirstOrDefaultAsync() == null)
                return null;

            DateTime currentDate = new DateTime(year, 1, 1);
            
            foreach(var h in query) {

                if (h.Date == currentDate)
                {
                    switch (h.HolidayType)
                    {
                        case HolidayType.ExtraWorkingDay:
                            break;
                            
                        case HolidayType.OtherDay:
                            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                currentFreeDays++;
                                currentDate = currentDate.AddDays(1);
                                continue;
                            }
                            break;

                        default:
                            currentFreeDays++;
                            currentDate = currentDate.AddDays(1);
                            continue;
                    }

                    if (currentFreeDays > maxFreeDays)
                        maxFreeDays = currentFreeDays;

                    currentFreeDays = 0;

                    currentDate = currentDate.AddDays(1);
                }
                else
                {
                    while (currentDate < h.Date)
                    {
                        if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            currentFreeDays++;
                        }
                        else
                        {
                            if (currentFreeDays > maxFreeDays)
                                maxFreeDays = currentFreeDays;

                            currentFreeDays = 0;
                        }

                        currentDate = currentDate.AddDays(1);
                    }
                }
            }
            
            return new { Days = maxFreeDays };
        }

        public async Task<bool> IsEmpty()
        {
            var firstHoliday = await _context.Holidays.FirstOrDefaultAsync();

            return firstHoliday == null;
        }

        public async Task<bool> IsEmpty(string countryCode, int year)
        {
            var firstHoliday = await _context.Holidays.Include(h => h.Country).FirstOrDefaultAsync(h => h.Country.CountryCode == countryCode && h.Date.Year == year);

            return firstHoliday == null;
        }

        public async Task Update(Holiday holiday)
        {
            _context.Entry(holiday).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
