using AutoFixture;
using FlightManager.Application.Extensions;
using FlightManager.Domain.Shared;
using FlightManager.TestHelpers;
using FluentAssertions;
using Xunit;

namespace FlightManager.Application.Tests.Extensions
{
    public class FlightExtensionsTests
    {
        private readonly Fixture _fixture;

        public FlightExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ToDto_ValidFlightAndDistance_Success()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var distance = _fixture.Create<Distance>();

            // Act
            var flightDto = flight.ToDto(distance);

            // Assert
            flightDto.Id.Should().Be(flight.Id);
            flightDto.DepartureAirportId.Should().Be(flight.DepartureAirportId);
            flightDto.DestinationAirportId.Should().Be(flight.DestinationAirportId);
            flightDto.FlightDuration.Should().Be(flight.GetFlightDuration(distance));
            flightDto.FuelRequiredInLitres.Should().Be(flight.GetFuelRequired(distance).Litres);
            flightDto.DistanceInKilometers.Should().Be(distance.Kilometers);
            flightDto.CreatedAt.Should().Be(flight.CreatedAt);
        }
    }
}