using CountryHolidaysAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public interface IHolidayRepositoryExtension : IRepository<Holiday>
    {
        Task<IEnumerable<object>> GetGroupedByMonth(string countryCode, int? year);
        Task<IEnumerable<object>> GetDayStatus(string countryCode, int? day, int? month, int? year);
        Task<object> GetMaximumFreeDays(string countryCode, int year);
        Task<bool> IsEmpty();
        Task<bool> IsEmpty(string countryCode, int year);
    }
}
