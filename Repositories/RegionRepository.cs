using CountryHolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public class RegionRepository : IRepository<Region>
    {
        private readonly CountryHolidaysContext _context;

        public RegionRepository(CountryHolidaysContext context)
        {
            _context = context;
        }

        public async Task<Region> Create(Region region)
        {
            _context.Add(region);
            await _context.SaveChangesAsync();

            return region;
        }

        public async Task CreateRange(List<Region> entries)
        {
            await _context.AddRangeAsync(entries);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var regionToDelete = await _context.Regions.FindAsync(id);
            _context.Regions.Remove(regionToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Region>> Get()
        {
            return await _context.Regions.ToListAsync();
        }

        public async Task<Region> Get(int id)
        {
            return await _context.Regions.FindAsync(id);
        }

        public async Task Update(Region region)
        {
            _context.Entry(region).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
