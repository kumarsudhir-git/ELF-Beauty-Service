
using Brewery.Application.Interfaces;
using Brewery.Domain.Entities;
using Brewery.Domain.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Brewery.Infrastructure.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IMemoryCache _cache;
        private const string CacheKey = AppConstants.BreweryCacheKeyPrefix + "AllBrewery";

        public BreweryService(IHttpClientService httpClientService, IMemoryCache memoryCache)
        {
            _httpClientService = httpClientService;
            _cache = memoryCache;
        }

        public async Task<IEnumerable<BreweryEntity>> GetBreweriesAsync(int pageNumber, int pageSize)
        {
            string url = AppConstants.BaseApiUrl + AppConstants.GetAllBreweryDataEndPoint;

            // CACHE (10 minutes)
            if (!_cache.TryGetValue(CacheKey, out List<BreweryEntity> breweries))
            {
                breweries = await _httpClientService.GetAsync<List<BreweryEntity>>(url);

                _cache.Set(CacheKey, breweries, TimeSpan.FromMinutes(AppConstants.CacheDurationInMinutes));
            }

            IEnumerable<BreweryEntity> pagedBreweries = breweries.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return pagedBreweries;
        }

        public Task<BreweryEntity> GetBreweryDataAsync(Guid breweryId)
        {
            string url = AppConstants.BaseApiUrl + AppConstants.GetBreweryDataEndpoint.Replace("{obdb-id}", breweryId.ToString());
            return _httpClientService.GetAsync<BreweryEntity>(url);
        }

        public async Task<IEnumerable<BreweryEntity>> GetFilteredBreweriesAsync(BreweryQueryParams query)
        {
            var breweries = await FetchBreweries(query);

            breweries = ApplySearch(breweries, query.Search);

            breweries = ApplySorting(breweries, query.SortBy);

            return breweries;
        }

        private string BuildUrl(BreweryQueryParams query)
        {
            var constructUrl = AppConstants.BaseApiUrl + AppConstants.GetAllBreweryDataEndPoint;

            var queryParams = new Dictionary<string, string>()
            {
                ["by_city"] = query.ByCity,
                ["by_country"] = query.ByCountry,
                ["by_dist"] = query.ByDist,
                ["by_ids"] = query.ByIds,
                ["by_name"] = query.ByName,
                ["by_state"] = query.ByState,
                ["by_postal"] = query.ByPostal,
                ["by_type"] = query.ByType,
                ["page"] = query.Page.ToString(),
                ["per_page"] = query.PerPage.ToString(),
                ["sort"] = query.Sort
            };

            // Remove nulls (important)
            var filteredParams = queryParams
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return QueryHelpers.AddQueryString(constructUrl, filteredParams!);
        }

        // FETCH
        private async Task<List<BreweryEntity>> FetchBreweries(BreweryQueryParams query)
        {
            var url = BuildUrl(query);

            var breweries = await _httpClientService.GetAsync<List<BreweryEntity>>(url);

            return breweries;
        }

        // SEARCH METHOD (SEPARATE)
        private List<BreweryEntity> ApplySearch(List<BreweryEntity> breweries, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return breweries;

            return breweries
                .Where(x =>
                    x.id.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.city.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.country.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.state.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.street.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.brewery_type.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.postal_code.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.state_province.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.brewery_type.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // SORTING METHOD (SEPARATE)
        private List<BreweryEntity> ApplySorting(List<BreweryEntity> breweries, string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name" => breweries.OrderBy(x => x.name).ToList(),
                "city" => breweries.OrderBy(x => x.city).ToList(),
                "country" => breweries.OrderBy(x => x.country).ToList(),
                _ => breweries
            };
        }

        //// BUILD EXTERNAL API URL
        //private string BuildUrl(BreweryQueryParams query)
        //{
        //    var baseUrl = "https://api.openbrewerydb.org/v1/breweries";

        //    var queryParams = new Dictionary<string, string?>();

        //    if (!string.IsNullOrEmpty(query.Country))
        //        queryParams["by_country"] = query.Country;

        //    if (query.PerPage.HasValue)
        //        queryParams["per_page"] = query.PerPage.Value.ToString();

        //    return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(baseUrl, queryParams!);
        //}

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // km
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
    }
}
