using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services.RepositoryServices
{
    public interface IHolidayService
    {
        Task<IEnumerable<object>> GetGroupedByMonthHolidays(string countryCode, int year);
        Task<object> GetDayStatus(string countryCode, int day, int month, int year);
        Task<object> GetMaxFreeDays(string countryCode, int year);
    }
}
