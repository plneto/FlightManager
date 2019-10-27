using System.Linq;
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
    public class FlightAllTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IFlightRepository> _flightRepositoryMock;
        private readonly Mock<IFlightDomainService> _flightDomainServiceMock;

        public FlightAllTests()
        {
            _fixture = new Fixture();
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _flightDomainServiceMock = new Mock<IFlightDomainService>();
        }

        [Fact]
        public async Task FlightAll_ValidQuery_Success()
        {
            // Arrange
            var query = new FlightAll.Query();
            var handler = new FlightAll.Handler(_flightRepositoryMock.Object, _flightDomainServiceMock.Object);

            var expectedFlights = DomainHelpers.CreateManyValidFlights().ToList();

            _flightRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedFlights);

            _flightDomainServiceMock
                .Setup(x => x.GetDistanceBetweenAirportsAsync(It.IsAny<Flight>()))
                .ReturnsAsync(_fixture.Create<Distance>());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Count().Should().Be(expectedFlights.Count);
        }
    }
}