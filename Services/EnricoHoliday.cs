using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services
{
    public class EnricoHoliday
    {
        public EnricoDate date { get; set; }
        public List<EnricoHolidayName> name { get; set; }
        public List<EnricoNote> note { get; set; }
        public List<string> flags { get; set; }
        public string holidayType { get; set; }
    }
}
