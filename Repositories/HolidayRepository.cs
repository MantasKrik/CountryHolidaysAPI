using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public class HolidayRepository : IRepository<Holiday>
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

        public async Task Update(Holiday holiday)
        {
            _context.Entry(holiday).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
