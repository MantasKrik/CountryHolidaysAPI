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
        Task<IEnumerable<object>> GetGroupedByMonth(string countryName, string year);
    }
}
