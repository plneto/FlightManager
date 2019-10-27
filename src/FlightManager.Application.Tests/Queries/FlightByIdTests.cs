using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.Queries;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Contracts.Services;
using FlightManager.Domain.Shared;
using FlightManager.TestHelpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.Queries
{
    public class FlightByIdTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IFlightRepository> _flightRepositoryMock;
        private readonly Mock<IFlightDomainService> _flightDomainServiceMock;

        public FlightByIdTests()
        {
            _fixture = new Fixture();
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _flightDomainServiceMock = new Mock<IFlightDomainService>();
        }

        [Fact]
        public async Task FlightById_ValidQuery_Success()
        {
            // Arrange
            var expectedFlight = DomainHelpers.CreateValidFlight();

            var query = new FlightById.Query(expectedFlight.Id);
            var handler = new FlightById.Handler(_flightRepositoryMock.Object, _flightDomainServiceMock.Object);

            _flightRepositoryMock
                .Setup(x => x.GetByIdAsync(expectedFlight.Id))
                .ReturnsAsync(expectedFlight);

            _flightDomainServiceMock
                .Setup(x => x.GetDistanceBetweenAirportsAsync(It.IsAny<Flight>()))
                .ReturnsAsync(_fixture.Create<Distance>());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(expectedFlight.Id);
        }

        [Fact]
        public void FlightById_EmptyId_ThrowsArgumentNullException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Action action = () => new FlightById.Query(id);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}