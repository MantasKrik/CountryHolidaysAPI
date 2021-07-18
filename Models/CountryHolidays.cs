using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class CountryHolidays : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }

        public CountryHolidays(DbContextOptions<CountryHolidays> options)
            : base(options)
        { }
    }
}
