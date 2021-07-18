using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public class HolidayRepository : IRepository<HolidayName>
    {
        private readonly CountryHolidaysContext _context;

        public HolidayRepository(CountryHolidaysContext context)
        {
            _context = context;
        }

        public async Task<HolidayName> Create(HolidayName holidayName)
        {
            _context.Add(holidayName);
            await _context.SaveChangesAsync();

            return holidayName;
        }

        public async Task Delete(int id)
        {
            var holidayToDelete = await _context.Holidays.FindAsync(id);
            _context.Holidays.Remove(holidayToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HolidayName>> Get()
        {
            return await _context.Holidays.ToListAsync();
        }

        public async Task<HolidayName> Get(int id)
        {
            return await _context.Holidays.FindAsync(id);
        }

        public async Task Update(HolidayName holidayName)
        {
            _context.Entry(holidayName).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
