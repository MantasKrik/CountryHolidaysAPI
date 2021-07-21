using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(20)]
        public string CountryCode { get; set; }
        [Required]
        public DateTime SupportedFromDate { get; set; }
        [Required]
        public DateTime SupportedToDate { get; set; }
        public List<Region> Regions { get; set; } = new List<Region>();
    }
}
