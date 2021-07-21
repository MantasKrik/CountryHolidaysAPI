using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            return await query.Select(h => new { status = h.HolidayType } ).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetGroupedByMonth(string countryCode, int? year)
        {
            IQueryable<Holiday> query = _context.Holidays;

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

            var query = _context.Holidays.Include(h => h.Country)
                .Where(h => h.Country.CountryCode.Equals(countryCode))
                .Where(h => h.Date.Year.Equals(year));
        
            for(DateTime date = new DateTime(year, 1, 1); date < new DateTime(year + 1, 1, 1); date.AddDays(1))
            {
                Holiday holiday = await query.FirstOrDefaultAsync(h => h.Date.Day == date.Day);
                if(holiday == null)
                {
                    if(date.DayOfWeek < DayOfWeek.Saturday)
                    {
                        currentFreeDays++;
                        continue;
                    }
                }
                else
                {
                    switch (holiday.HolidayType)
                    {
                        case HolidayType.ExtraWorkingDay:
                        case HolidayType.OtherDay:
                            currentFreeDays++;
                            continue;
                    }

                    if (currentFreeDays > maxFreeDays)
                    {
                        maxFreeDays = currentFreeDays;
                        currentFreeDays = 0;
                    }
                }
            }

            return maxFreeDays;
        }

        public async Task Update(Holiday holiday)
        {
            _context.Entry(holiday).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
