using System.Collections.Generic;
using System.Threading.Tasks;
using CountryHolidaysAPI.Services.RepositoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidaysController(IHolidayService holidayService)
        {
            this._holidayService = holidayService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<object>>> GetHolidays(int? year, string countryCode = null)
        {
            if (!year.HasValue || string.IsNullOrEmpty(countryCode))
                return BadRequest("year and country code has to be provided. Example: /year=2021&countryCode=ltu");

            var response = await this._holidayService.GetGroupedByMonthHolidays(countryCode, year.Value);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("status/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<object>>> GetDayStatus(int? day, int? month, int? year, string countryCode = null)
        {
            if(!day.HasValue || !month.HasValue || !year.HasValue || string.IsNullOrEmpty(countryCode))
                return BadRequest("year, month, day and country code has to be provided. Example: /day=1&month=1&year=2021&countryCode=ltu");

            var response = await this._holidayService.GetDayStatus(countryCode, day.Value, month.Value, year.Value);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("free/max")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetFreeDays(int? year, string countryCode = null)
        {
            if (!year.HasValue || string.IsNullOrEmpty(countryCode))
                return BadRequest("year and country code has to be provided. Example: /max?year=2021&countryCode=ltu");

            var response = await this._holidayService.GetMaxFreeDays(countryCode, year.Value);

            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}