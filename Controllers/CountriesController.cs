using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CountryHolidaysAPI.Models;
using CountryHolidaysAPI.Repositories;
using CountryHolidaysAPI.Services.RepositoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(IRepository<Country> countryRepository, ICountryService countryService)
        {
            this._countryService = countryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            var response = await _countryService.GetCountries();

            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}