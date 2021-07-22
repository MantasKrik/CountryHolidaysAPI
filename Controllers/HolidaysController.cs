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
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayRepositoryExtension _holidayRepository;

        public HolidaysController(IHolidayRepositoryExtension holidayRepository)
        {
            this._holidayRepository = holidayRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<object>> GetHolidays(int? year, string countryName = null)
        {
            return await this._holidayRepository.GetGroupedByMonth(countryName, year);
        }

        [HttpGet("status/")]
        public async Task<IEnumerable<object>> GetDayStatus(int? day, int? month, int? year, string countryCode = null)
        {
            return await this._holidayRepository.GetDayStatus(countryCode, day, month, year);
        }

        [HttpGet("free/max")]
        public async Task<ActionResult<object>> GetFreeDays(int? year, string countryCode = null)
        {
            if (!year.HasValue || string.IsNullOrEmpty(countryCode))
                return BadRequest("year and country code has to be provided. Example: /max?year=2021&countryCode=ltu");

            var response = await this._holidayRepository.GetMaximumFreeDays(countryCode, year.Value);

            if (response == null)
                return NotFound();

            return response;
        }
    }
}