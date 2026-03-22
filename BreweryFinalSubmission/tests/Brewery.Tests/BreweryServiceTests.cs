using Xunit;
using Brewery.Infrastructure.Services;
using System.Threading.Tasks;
using Moq;
using Brewery.Application.Interfaces;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace Brewery.Tests
{
    public class BreweryServiceTests
    {
        private Mock<IHttpClientService> _httpClientServiceMock;
        private Mock<IMemoryCache> _memoryCacheMock;

        public BreweryServiceTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _memoryCacheMock = new Mock<IMemoryCache>();
        }

        [Fact]
        public async Task Should_Return_Data()
        {
            Guid breweryId = new Guid("b54b16e1-ac3b-4bff-a11f-f7ae9ddc27e0"); // Set a valid obdb-id for testing
            var service = new BreweryService(_httpClientServiceMock.Object, _memoryCacheMock.Object);
            var result = await service.GetBreweryDataAsync(breweryId);
            Assert.NotNull(result);
        }
    }
}
