using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class CountryHolidays : DbContext
    {
        public CountryHolidays(DbContextOptions<CountryHolidays> options)
            : base(options)
        { }
    }
}
