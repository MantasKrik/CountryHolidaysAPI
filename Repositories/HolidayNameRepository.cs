using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public class HolidayNameRepository : IRepository<HolidayName>
    {
        private readonly CountryHolidaysContext _context;

        public HolidayNameRepository(CountryHolidaysContext context)
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
            var holidayNameToDelete = await _context.HolidayNames.FindAsync(id);
            _context.HolidayNames.Remove(holidayNameToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HolidayName>> Get()
        {
            return await _context.HolidayNames.ToListAsync();
        }

        public async Task<HolidayName> Get(int id)
        {
            return await _context.HolidayNames.FindAsync(id);
        }

        public async Task Update(HolidayName holidayName)
        {
            _context.Entry(holidayName).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
