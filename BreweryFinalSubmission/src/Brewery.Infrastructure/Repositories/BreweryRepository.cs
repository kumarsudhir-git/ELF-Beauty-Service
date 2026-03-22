using Brewery.Application.Interfaces;
using Brewery.Domain.Entities;
using Brewery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Infrastructure.Repositories
{
    public class BreweryRepository : IBreweryRepository
    {
        private readonly AppDbContext _context;

        public BreweryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BreweryEntity>> GetAllAsync()
        {
            return await _context.Breweries.ToListAsync();
        }

        public async Task<BreweryEntity> GetByIdAsync(int id)
        {
            return await _context.Breweries.FindAsync(id);
        }

        public async Task AddAsync(BreweryEntity brewery)
        {
            await _context.Breweries.AddAsync(brewery);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
