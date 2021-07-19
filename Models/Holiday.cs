using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public enum HolidayType
    {
        PublicHoliday,
        Observance,
        SchoolHoliday,
        OtherDay,
        ExtraWorkingDay
    }

    public class Holiday
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<HolidayName> HolidayNames { get; set; }
        public HolidayType HolidayType { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public Region? Region { get; set; }
    }
}
