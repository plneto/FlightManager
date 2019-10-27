using System;
using System.Linq;
using AutoFixture;
using FlightManager.Domain.AggregatesModel.AirportAggregate;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.AggregateModels.AirportAggregate
{
    public class CoordinatesTests
    {
        private readonly double _validLatitude;
        private readonly double _validLongitude;

        public CoordinatesTests()
        {
            var fixture = new Fixture();
            var doubleGenerator = new Generator<double>(fixture);

            _validLatitude = doubleGenerator.First(x => x >= -90 && x <= 90);
            _validLongitude = doubleGenerator.First(x => x >= -180 && x <= 180);
        }

        [Fact]
        public void InitializeCoordinates_ValidParameters_Success()
        {
            // Arrange & Act
            var result = new Coordinates(_validLatitude, _validLongitude);

            // Assert
            result.Latitude.Should().Be(_validLatitude);
            result.Longitude.Should().Be(_validLongitude);
        }

        [Theory]
        [InlineData(-91)]
        [InlineData(91)]
        public void InitializeCoordinates_InvalidLatitude_ThrowsArgumentException(double latitude)
        {
            // Arrange & Act
            Action action = () => new Coordinates(latitude, _validLongitude);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-181)]
        [InlineData(181)]
        public void InitializeCoordinates_InvalidLongitude_ThrowsArgumentException(double longitude)
        {
            // Arrange & Act
            Action action = () => new Coordinates(_validLatitude, longitude);

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}