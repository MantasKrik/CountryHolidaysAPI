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
        private readonly IRepository<Holiday> _holidayRepository;

        public HolidaysController(IRepository<Holiday> holidayRepository)
        {
            this._holidayRepository = holidayRepository;
        }
    }
}