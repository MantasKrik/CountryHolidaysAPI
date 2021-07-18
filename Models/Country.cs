using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CountryCode { get; set; }
        public DateTime SupportedFromDate { get; set; }
        public DateTime SupportedToDate { get; set; }
    }
}
