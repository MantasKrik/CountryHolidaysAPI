using Microsoft.EntityFrameworkCore;

namespace CountryHolidaysAPI.Models
{
    public class CountryHolidaysContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayName> HolidayNames { get; set; }

        public CountryHolidaysContext(DbContextOptions<CountryHolidaysContext> options)
            : base(options)
        { }
    }
}
