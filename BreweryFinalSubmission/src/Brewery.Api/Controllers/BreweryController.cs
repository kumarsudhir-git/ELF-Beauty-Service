using Brewery.Application.Interfaces;
using Brewery.Domain.Entities;
using Brewery.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    public class BreweryController : ControllerBase
    {
        private readonly IBreweryService _breweryService;
        private readonly ILogger<BreweryController> _logger;

        public BreweryController(IBreweryService breweryService, ILogger<BreweryController> logger)
        {
            this._breweryService = breweryService;
            this._logger = logger;
        }

        [HttpGet("api/brewery/{breweryId}")]
        public async Task<ActionResult<BreweryEntity>> GetBreweryData(Guid breweryId)
        {
            BreweryEntity breweryEntity = await _breweryService.GetBreweryDataAsync(breweryId);
            if (breweryEntity == null)
            {
                _logger.LogWarning($"Brewery with ID {breweryId} not found.");
                return NotFound();
            }
            return Ok(breweryEntity);
        }

        [HttpGet("api/breweriesByPage/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<BreweryEntity>>> GetAllBreweryDataAsync(int pageNumber = 1, int pageSize = 20)
        {
            IEnumerable<BreweryEntity> breweries = await _breweryService.GetBreweriesAsync(pageNumber, pageSize);
            if (breweries == null)
            {
                _logger.LogWarning("No data found");
                return NotFound();
            }
            return Ok(breweries);
        }

        [HttpGet("api/breweries")]
        public async Task<IActionResult> Get([FromQuery] BreweryQueryParams query)
        {
            var result = await _breweryService.GetFilteredBreweriesAsync(query);
            if (result == null)
            {
                _logger.LogWarning($"No data found for search: {query}");
                return NotFound();
            }
            return Ok(result);
        }
    }
}
