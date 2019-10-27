using System;
using AutoFixture;
using FlightManager.Domain.AggregatesModel.AirportAggregate;
using FlightManager.TestHelpers;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.AggregateModels.AirportAggregate
{
    public class AirportTests
    {
        private readonly Fixture _fixture;

        private readonly double _validLatitude;
        private readonly double _validLongitude;

        public AirportTests()
        {
            _fixture = new Fixture();

            _validLatitude = DomainHelpers.GetValidLatitude();
            _validLongitude = DomainHelpers.GetValidLongitude();
        }

        [Fact]
        public void InitializeAirport_ValidParameters_Success()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var code = _fixture.Create<string>();

            // Act
            var airport = new Airport(name, code, _validLatitude, _validLongitude);

            // Assert
            airport.Id.Should().NotBe(default);
            airport.Name.Should().BeEquivalentTo(name);
            airport.Code.Should().BeEquivalentTo(code);
            airport.Coordinates.Latitude.Should().Be(_validLatitude);
            airport.Coordinates.Longitude.Should().Be(_validLongitude);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void InitializeAirport_EmptyOrNullName_ThrowsArgumentNullException(string name)
        {
            // Arrange
            var code = _fixture.Create<string>();

            // Act
            Action action = () => new Airport(name, code, _validLatitude, _validLongitude);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void InitializeAirport_EmptyOrNullCode_ThrowsArgumentNullException(string code)
        {
            // Arrange
            var name = _fixture.Create<string>();

            // Act
            Action action = () => new Airport(name, code, _validLatitude, _validLongitude);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Airport_GetDistanceTo_Success()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var code = _fixture.Create<string>();
            var name2 = _fixture.Create<string>();
            var code2 = _fixture.Create<string>();

            var airport = new Airport(name, code, _validLatitude, _validLongitude);
            var airport2 = new Airport(name2, code2, _validLatitude, _validLongitude);

            // Act
            var distance = airport.GetDistanceTo(airport2);

            // Assert
            distance.Should().NotBeNull();
        }
    }
}