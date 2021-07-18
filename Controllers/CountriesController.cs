using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountryHolidaysAPI.Models;
using CountryHolidaysAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IRepository<Country> _countryRepository;

        public CountriesController(IRepository<Country> countryRepository)
        {
            this._countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _countryRepository.Get();
        }
    }
}