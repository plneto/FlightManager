using AutoFixture;
using FlightManager.Domain.Shared;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.Shared
{
    public class DistanceTests
    {
        private readonly Fixture _fixture;

        public DistanceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void InitializeDistance_FromConstructor_Success()
        {
            // Arrange
            var meters = _fixture.Create<double>();

            // Act
            var distance = new Distance(meters);

            // Assert
            distance.Should().NotBeNull();
            distance.Meters.Should().Be(meters);
            distance.Meters.Should().Be(distance.Kilometers * 1000.0);
        }

        [Fact]
        public void InitializeDistance_FromMetersFactory_Success()
        {
            // Arrange
            var meters = _fixture.Create<double>();

            // Act
            var distance = Distance.FromMeters(meters);

            // Assert
            distance.Should().NotBeNull();
            distance.Meters.Should().Be(meters);
            distance.Meters.Should().Be(distance.Kilometers * 1000.0);
        }

        [Fact]
        public void InitializeDistance_FromKilometersFactory_Success()
        {
            // Arrange
            var kilometers = _fixture.Create<double>();

            // Act
            var distance = Distance.FromKilometers(kilometers);

            // Assert
            distance.Should().NotBeNull();
            distance.Kilometers.Should().Be(kilometers);
            distance.Kilometers.Should().Be(distance.Meters / 1000.0);
        }
    }
}