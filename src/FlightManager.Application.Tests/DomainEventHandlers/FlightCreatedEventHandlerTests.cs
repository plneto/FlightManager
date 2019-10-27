using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.DomainEventHandlers;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using FlightManager.TestHelpers;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.DomainEventHandlers
{
    public class FlightCreatedEventHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IReportItemRepository> _reportItemRepositoryMock;
        private readonly Mock<IFlightRepository> _flightRepositoryMock;
        private readonly Mock<IAirportRepository> _airportRepositoryMock;

        public FlightCreatedEventHandlerTests()
        {
            _fixture = new Fixture();
            _reportItemRepositoryMock = new Mock<IReportItemRepository>();
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _airportRepositoryMock = new Mock<IAirportRepository>();
        }

        [Fact]
        public async Task FlightCreatedEventHandler_ValidEvent_Success()
        {
            // Arrange
            var expectedEvent = _fixture.Create<FlightCreatedEvent>();
            var expectedFlight = DomainHelpers.CreateValidFlight();

            _flightRepositoryMock
                .Setup(x => x.GetByIdAsync(expectedEvent.Id))
                .ReturnsAsync(expectedFlight);

            _airportRepositoryMock
                .Setup(x => x.GetByIdAsync(expectedFlight.DepartureAirportId))
                .ReturnsAsync(DomainHelpers.CreateValidAirport());

            _airportRepositoryMock
                .Setup(x => x.GetByIdAsync(expectedFlight.DestinationAirportId))
                .ReturnsAsync(DomainHelpers.CreateValidAirport());

            var target = new FlightCreatedEventHandler(
                _reportItemRepositoryMock.Object,
                _flightRepositoryMock.Object,
                _airportRepositoryMock.Object);

            // Act
            await target.Handle(expectedEvent, CancellationToken.None);

            // Assert
            _reportItemRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<ReportItem>()),
                Times.Once);

            _flightRepositoryMock.Verify();
            _airportRepositoryMock.Verify();
        }
    }
}