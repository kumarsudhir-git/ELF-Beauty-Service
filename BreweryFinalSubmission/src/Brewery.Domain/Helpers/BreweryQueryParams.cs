namespace Brewery.Domain.Helpers
{
    public class BreweryQueryParams
    {
        public string ByCity { get; set; }
        public string ByCountry { get; set; }
        public string ByDist { get; set; } // "lat,long"
        public string ByIds { get; set; }
        public string ByName { get; set; }
        public string ByState { get; set; }
        public string ByPostal { get; set; }
        public string ByType { get; set; }

        public int? Page { get; set; }
        public int? PerPage { get; set; }

        public string Sort { get; set; }

        public string Search { get; set; }   // custom search
        public string SortBy { get; set; }  // name | city | distance
    }
}
