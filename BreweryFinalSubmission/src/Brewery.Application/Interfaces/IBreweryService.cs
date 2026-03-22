using Brewery.Domain.Entities;
using Brewery.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Application.Interfaces
{
    public interface IBreweryService
    {
        Task<BreweryEntity> GetBreweryDataAsync(Guid breweryId);
        Task<IEnumerable<BreweryEntity>> GetBreweriesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<BreweryEntity>> GetFilteredBreweriesAsync(BreweryQueryParams query);
    }
}
