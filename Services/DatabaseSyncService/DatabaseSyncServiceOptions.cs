namespace CountryHolidaysAPI.Services
{
    public class DatabaseSyncServiceOptions
    {
        public const string DatabaseSyncService = "DatabaseSyncService";

        public double SyncDelayHours { get; set; }
        public bool RegisterService { get; set; }
    }
}
