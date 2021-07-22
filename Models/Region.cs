using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Models
{
    public class Region
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string RegionCode { get; set; }
        
        [Required]
        [MaxLength(255)]
        [JsonIgnore]
        public Country Country { get; set; }
        public int CountryId { get; set; }
    }
}
