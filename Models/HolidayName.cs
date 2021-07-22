using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Holiday Holiday { get; set; }
        public int HolidayId { get; set; }
    }
}
