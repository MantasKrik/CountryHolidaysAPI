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

        public async Task<IEnumerable<object>> GetGroupedByMonth(string countryCode, string year)
        {
            IQueryable<Holiday> query = _context.Holidays;

            if (!string.IsNullOrEmpty(countryCode))
                query = query.Where(h => h.Country.CountryCode.Equals(countryCode));

            if (!string.IsNullOrEmpty(year))
                query = query.Where(h => h.Date.Year.Equals(year));

            return await query
                .GroupBy(h => h.Date.Month)
                .Select(g => new { g.Key, holidays = g.Select(h => h.Date.Month == g.Key) })
                .ToListAsync();
        }

        public async Task Update(Holiday holiday)
        {
            _context.Entry(holiday).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
