using CountryHolidaysAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Repositories
{
    public interface ICountryRepositoryExtension : IRepository<Country>
    {
        Task<Country> Get(string countryCode);
        Task<bool> IsEmpty();
    }
}
