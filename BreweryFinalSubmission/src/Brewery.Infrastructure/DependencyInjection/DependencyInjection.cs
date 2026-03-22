using Brewery.Application.Interfaces;
using Brewery.Infrastructure.ExternalServices;
using Brewery.Infrastructure.Persistence;
using Brewery.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brewery.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            //services.AddScoped<IBreweryRepository, BreweryRepository>();

            // ✅ Services
            services.AddScoped<IBreweryService, BreweryService>();

            // HttpClient
            services.AddHttpClient<IHttpClientService, HttpClientService>();

            // Memory Cache
            services.AddMemoryCache();

            return services;
        }
    }
}
