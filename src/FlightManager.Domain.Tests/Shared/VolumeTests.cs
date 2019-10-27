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
        }
    }
}