using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services
{
    public class DatabaseSyncServiceOptions
    {
        public const string DatabaseSyncService = "DatabaseSyncService";

        public double SyncDelayHours { get; set; }
        public bool RegisterService { get; set; }
    }
}
