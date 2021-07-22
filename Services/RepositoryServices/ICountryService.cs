using CountryHolidaysAPI.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services.RepositoryServices
{
    public interface ICountryService
    {
       Task<IEnumerable<Country>> GetCountries();
       Task SyncCountries(); 
    }
}
