using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class HolidayName
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Language { get; set; }
        [MaxLength(255)]
        public string Text { get; set; }
        [Required]
        public Holiday Holiday { get; set; }
        public int HolidayId { get; set; }
    }
}
