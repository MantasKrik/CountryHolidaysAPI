using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public class CountryRepository : IRepository<Country>
    {
        private readonly CountryHolidaysContext _context;

        public CountryRepository(CountryHolidaysContext context)
        {
            _context = context;
        }

        public async Task<Country> Create(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();

            return country;
        }

        public async Task Delete(int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(countryToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Country>> Get()
        {
            return await _context.Countries.Include(c => c.Regions).ToListAsync();
        }

        public async Task<Country> Get(int id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task Update(Country country)
        {
            _context.Entry(country).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

