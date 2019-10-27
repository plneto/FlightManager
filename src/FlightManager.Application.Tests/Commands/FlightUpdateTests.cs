using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.Commands;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using FlightManager.TestHelpers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.Commands
{
    public class FlightUpdateTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IFlightRepository> _flightRepository;

        public FlightUpdateTests()
        {
            _fixture = new Fixture();
            _mediatorMock = new Mock<IMediator>();
            _flightRepository = new Mock<IFlightRepository>();
        }

        [Fact]
        public async Task FlightUpdate_ValidCommand_CommandHandledWithSuccess()
        {
            // Arrange
            var expectedFlight = DomainHelpers.CreateValidFlight();

            var command = new FlightUpdate.Command(
                expectedFlight.Id,
                Guid.NewGuid(),
                Guid.NewGuid());

            var handler = new FlightUpdate.Handler(_mediatorMock.Object, _flightRepository.Object);

            _flightRepository
                .Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(expectedFlight);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _flightRepository.Verify(x => x.UpdateAsync(command.Id, expectedFlight), Times.Once);

            _mediatorMock.Verify(x => x.Publish<INotification>(
                It.IsAny<FlightDepartureAirportUpdatedEvent>(),
                It.IsAny<CancellationToken>()));

            _mediatorMock.Verify(x => x.Publish<INotification>(
                It.IsAny<FlightDestinationAirportUpdatedEvent>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void FlightUpdate_DefaultFlightId_ThrowsArgumentNullException()
        {
            // Arrange
            var flightId = Guid.Empty;
            var departureAirportId = _fixture.Create<Guid>();
            var destinationAirportId = _fixture.Create<Guid>();

            // Act
            Action action = () => new FlightUpdate.Command(
                flightId,
                departureAirportId,
                destinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FlightUpdate_DefaultDepartureAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var flightId = _fixture.Create<Guid>();
            var departureAirportId = Guid.Empty;
            var destinationAirportId = _fixture.Create<Guid>();

            // Act
            Action action = () => new FlightUpdate.Command(
                flightId,
                departureAirportId,
                destinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FlightUpdate_DefaultDestinationAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var flightId = _fixture.Create<Guid>();
            var departureAirportId = _fixture.Create<Guid>();
            var destinationAirportId = Guid.Empty;

            // Act
            Action action = () => new FlightUpdate.Command(
                flightId,
                departureAirportId,
                destinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}