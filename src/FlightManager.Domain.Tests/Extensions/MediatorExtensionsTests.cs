using System;
using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.Events;
using FlightManager.Domain.Extensions;
using FlightManager.TestHelpers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace FlightManager.Domain.Tests.Extensions
{
    public class MediatorExtensionsTests
    {
        private readonly Mock<IMediator> _mediatorMock;

        public MediatorExtensionsTests()
        {
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task DispatchDomainEventsAsync_EventsCreated_PublishEventsAndEmptyList()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            flight.ChangeDestinationAirport(Guid.NewGuid());

            // Act
            await _mediatorMock.Object.DispatchDomainEventsAsync(flight);

            // Assert
            _mediatorMock.Verify(x => x.Publish<INotification>(
                It.IsAny<FlightCreatedEvent>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _mediatorMock.Verify(x => x.Publish<INotification>(
                    It.IsAny<FlightDestinationAirportUpdatedEvent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            flight.DomainEvents.Should().BeEmpty();
        }
    }
}