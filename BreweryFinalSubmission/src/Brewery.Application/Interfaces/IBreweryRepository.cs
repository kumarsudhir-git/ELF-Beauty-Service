using Brewery.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Application.Interfaces
{
    public interface IBreweryRepository
    {
        Task<IEnumerable<BreweryEntity>> GetAllAsync();
        Task<BreweryEntity> GetByIdAsync(int id);
        Task AddAsync(BreweryEntity brewery);
        Task SaveChangesAsync();
    }
}
