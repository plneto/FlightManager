using AutoFixture;
using FlightManager.Domain.Shared;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.Shared
{
    public class VolumeTests
    {
        private readonly Fixture _fixture;

        public VolumeTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void InitializeVolume_FromConstructor_Success()
        {
            // Arrange
            var litres = _fixture.Create<double>();

            // Act
            var volume = new Volume(litres);

            // Assert
            volume.Should().NotBeNull();
            volume.Litres.Should().Be(litres);
            volume.Litres.Should().Be(volume.Gallons / 0.264);
        }

        [Fact]
        public void InitializeVolume_FromLitresFactory_Success()
        {
            // Arrange
            var litres = _fixture.Create<double>();

            // Act
            var volume = Volume.FromLitres(litres);

            // Assert
            volume.Should().NotBeNull();
            volume.Litres.Should().Be(litres);
            volume.Litres.Should().Be(volume.Gallons / 0.264);
        }

        [Fact]
        public void InitializeVolume_FromGallonsFactory_Success()
        {
            // Arrange
            var gallons = _fixture.Create<double>();

            // Act
            var volume = Volume.FromGallons(gallons);

            // Assert
            volume.Should().NotBeNull();
            volume.Gallons.Should().Be(gallons);
            volume.Gallons.Should().Be(volume.Litres * 0.264);
        }
    }
}