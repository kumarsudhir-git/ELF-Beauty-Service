using Xunit;
using Brewery.Infrastructure.Services;
using System.Threading.Tasks;
using Moq;
using Brewery.Application.Interfaces;
using System;

namespace Brewery.Tests
{
    public class BreweryServiceTests
    {
        private Mock<IHttpClientService> _httpClientServiceMock;

        public BreweryServiceTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
        }

        [Fact]
        public async Task Should_Return_Data()
        {
            Guid breweryId = new Guid("b54b16e1-ac3b-4bff-a11f-f7ae9ddc27e0"); // Set a valid obdb-id for testing
            var service = new BreweryService(_httpClientServiceMock.Object);
            var result = await service.GetBreweryDataAsync(breweryId);
            Assert.NotNull(result);
        }
    }
}
