using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class HolidayName
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }
        public Holiday Holiday { get; set; }
        public int HolidayId { get; set; }
    }
}
