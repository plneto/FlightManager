using System.Threading.Tasks;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Services;
using FlightManager.TestHelpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace FlightManager.Domain.Tests.Services
{
    public class FlightDomainServiceTests
    {
        private readonly Mock<IAirportRepository> _airportRepositoryMock;

        public FlightDomainServiceTests()
        {
            _airportRepositoryMock = new Mock<IAirportRepository>();
        }

        [Fact]
        public async Task GetDistanceBetweenAirportsAsync_ValidFlight_ReturnsDistance()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();

            _airportRepositoryMock
                .Setup(x => x.GetByIdAsync(flight.DepartureAirportId))
                .ReturnsAsync(DomainHelpers.CreateValidAirport());

            _airportRepositoryMock
                .Setup(x => x.GetByIdAsync(flight.DestinationAirportId))
                .ReturnsAsync(DomainHelpers.CreateValidAirport());

            var target = new FlightDomainService(_airportRepositoryMock.Object);

            // Act
            var distance = await target.GetDistanceBetweenAirportsAsync(flight);

            // Assert
            distance.Should().NotBeNull();
        }
    }
}