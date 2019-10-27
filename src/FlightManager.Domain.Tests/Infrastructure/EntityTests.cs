using AutoFixture;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Events;
using FlightManager.TestHelpers;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.Infrastructure
{
    public class EntityTests
    {
        private readonly Fixture _fixture;

        public EntityTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Entity_NoEventsAdded_DomainEventsIsNull()
        {
            // Arrange
            var reportItem = _fixture.Create<ReportItem>();

            // Act & Assert
            reportItem.DomainEvents.Should().BeNull();
        }

        [Fact]
        public void AddDomainEvent_AddFirstEvent_CreatesDomainEventsList()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();

            // Act & Assert
            flight.DomainEvents.Should().NotBeNull();
            flight.DomainEvents.Should()
                .Contain(x => x.GetType() == typeof(FlightCreatedEvent))
                .And.HaveCount(1);
        }

        [Fact]
        public void ClearDomainEvents_RemoveEvents_Success()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            flight.ClearDomainEvents();

            // Assert
            flight.DomainEvents.Should().BeEmpty();
        }
    }
}