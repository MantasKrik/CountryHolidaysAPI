using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
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
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public List<HolidayName> HolidayNames { get; set; } = new List<HolidayName>();
        [Required]
        public HolidayType HolidayType { get; set; }
        [Required]
        [JsonIgnore]
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public Region Region { get; set; }
    }
}
