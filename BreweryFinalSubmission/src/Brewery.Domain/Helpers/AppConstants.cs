namespace Brewery.Domain.Helpers
{
    public class AppConstants
    {
        public const int DefaultPageSize = 20;
        public const int DefaultPageNumber = 1;
        public const int MaxRetryAttempts = 3;
        public const int CacheDurationInMinutes = 10;
        public const string BreweryCacheKeyPrefix = "BreweryData_";
        public const string BaseApiUrl = "https://api.openbrewerydb.org/";
        public const string GetBreweryDataEndpoint = "v1/breweries/{obdb-id}";
        public const string GetAllBreweryDataEndPoint = "v1/breweries";
    }
}
