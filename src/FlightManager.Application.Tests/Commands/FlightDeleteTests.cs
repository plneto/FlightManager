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
    public class FlightDeleteTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IFlightRepository> _flightRepository;

        public FlightDeleteTests()
        {
            _fixture = new Fixture();
            _mediatorMock = new Mock<IMediator>();
            _flightRepository = new Mock<IFlightRepository>();
        }

        [Fact]
        public async Task FlightDelete_ValidCommand_CommandHandledWithSuccess()
        {
            // Arrange
            var command = _fixture.Create<FlightDelete.Command>();
            var handler = new FlightDelete.Handler(_mediatorMock.Object, _flightRepository.Object);

            _flightRepository
                .Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(DomainHelpers.CreateValidFlight());

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _flightRepository.Verify(x => x.DeleteAsync(command.Id), Times.Once);

            _mediatorMock.Verify(x => x.Publish<INotification>(
                It.IsAny<FlightDeletedEvent>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void FlightDelete_DefaultFlightId_ThrowsArgumentNullException()
        {
            // Arrange
            var flightId = Guid.Empty;

            // Act
            Action action = () => new FlightDelete.Command(flightId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}