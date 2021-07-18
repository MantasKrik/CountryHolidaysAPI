using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class CountryHolidaysContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<HolidayName> Holidays { get; set; }
        public DbSet<HolidayName> HolidayNames { get; set; }

        public CountryHolidaysContext(DbContextOptions<CountryHolidaysContext> options)
            : base(options)
        { }
    }
}
