using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public Country Country { get; set; }
    }
}
