using CountryHolidaysAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services.RepositoryServices
{
    public interface ICountryService
    {
       Task<IEnumerable<Country>> GetCountries();
       Task SyncCountries(); 
    }
}
