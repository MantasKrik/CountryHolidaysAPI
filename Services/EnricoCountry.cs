using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services
{
    public class EnricoCountry
    {
        public string countryCode { get; set; }
        public List<string> regions { get; set; }
        public List<string> holidayTypes { get; set; }
        public string fullName { get; set; }
        public EnricoDate fromDate { get; set; }
        public EnricoDate toDate { get; set; }
    }
}
