using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.Commands;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.Commands
{
    public class FlightCreateTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IFlightRepository> _flightRepository;

        public FlightCreateTests()
        {
            _fixture = new Fixture();
            _mediatorMock = new Mock<IMediator>();
            _flightRepository = new Mock<IFlightRepository>();
        }

        [Fact]
        public async Task FlightCreate_ValidCommand_CommandHandledWithSuccess()
        {
            // Arrange
            var command = _fixture.Create<FlightCreate.Command>();
            var handler = new FlightCreate.Handler(_mediatorMock.Object, _flightRepository.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _flightRepository.Verify(x => x.AddAsync(
                It.Is<Flight>(y =>
                    y.DepartureAirportId == command.DepartureAirportId &&
                    y.DestinationAirportId == command.DestinationAirportId)),
                Times.Once);

            _mediatorMock.Verify(x => x.Publish<INotification>(
                It.IsAny<FlightCreatedEvent>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void FlightCreate_DefaultDepartureAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var departureAirportId = Guid.Empty;
            var destinationAirportId = this._fixture.Create<Guid>();

            // Act
            Action action = () => new FlightCreate.Command(departureAirportId, destinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FlightCreate_DefaultDestinationAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var departureAirportId = this._fixture.Create<Guid>();
            var destinationAirportId = Guid.Empty;

            // Act
            Action action = () => new FlightCreate.Command(departureAirportId, destinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}