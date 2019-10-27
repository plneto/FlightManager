using System;
using AutoFixture;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Events;
using FlightManager.Domain.Shared;
using FlightManager.TestHelpers;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.AggregateModels.FlightAggregate
{
    public class FlightTests
    {
        private readonly Fixture _fixture;

        public FlightTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void InitializeFlight_ValidParameters_Success()
        {
            // Arrange
            var departureAirportId = _fixture.Create<Guid>();
            var destinationAirportId = _fixture.Create<Guid>();

            // Act
            var flight = new Flight(departureAirportId, destinationAirportId);

            // Assert
            flight.Id.Should().NotBe(default);
            flight.DepartureAirportId.Should().Be(departureAirportId);
            flight.DestinationAirportId.Should().Be(destinationAirportId);
            flight.DomainEvents.Should()
                .Contain(x => x.GetType() == typeof(FlightCreatedEvent))
                .And.HaveCount(1);
        }

        [Fact]
        public void InitializeFlight_InvalidDepartureAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var emptyDepartureAirportId = Guid.Empty;
            var destinationAirportId = _fixture.Create<Guid>();

            // Act
            Action action = () => new Flight(emptyDepartureAirportId, destinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void InitializeFlight_InvalidDestinationAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var departureAirportId = _fixture.Create<Guid>();
            var emptyDestinationAirportId = Guid.Empty;

            // Act
            Action action = () => new Flight(departureAirportId, emptyDestinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void InitializeFlight_SameAirports_ThrowsArgumentException()
        {
            // Arrange
            var airportId = _fixture.Create<Guid>();

            // Act
            Action action = () => new Flight(airportId, airportId);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ChangeDepartureAirport_ValidParameters_Success()
        {
            // Arrange
            var newDepartureAirportId = _fixture.Create<Guid>();
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            flight.ChangeDepartureAirport(newDepartureAirportId);

            // Assert
            flight.DepartureAirportId.Should().Be(newDepartureAirportId);
            flight.DomainEvents
                .Should()
                .Contain(x => x.GetType() == typeof(FlightDepartureAirportUpdatedEvent));
        }

        [Fact]
        public void ChangeDepartureAirport_InvalidDepartureAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var newDepartureAirportId = Guid.Empty;
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            Action action = () => flight.ChangeDepartureAirport(newDepartureAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ChangeDepartureAirport_SameAirportAsDestination_ThrowsArgumentException()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            Action action = () => flight.ChangeDepartureAirport(flight.DestinationAirportId);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ChangeDepartureAirport_SameDepartureAirportId_ReturnWithoutMakingChanges()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var sameDepartureAirportId = flight.DepartureAirportId;

            // Act
            flight.ChangeDepartureAirport(sameDepartureAirportId);

            // Assert
            flight.DepartureAirportId.Should().Be(sameDepartureAirportId);
            flight.DomainEvents.Should()
                .NotContain(x => x.GetType() == typeof(FlightDepartureAirportUpdatedEvent));
        }

        [Fact]
        public void ChangeDestinationAirport_ValidParameters_Success()
        {
            // Arrange
            var newDestinationAirportId = _fixture.Create<Guid>();
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            flight.ChangeDestinationAirport(newDestinationAirportId);

            // Assert
            flight.DestinationAirportId.Should().Be(newDestinationAirportId);
            flight.DomainEvents
                .Should()
                .Contain(x => x.GetType() == typeof(FlightDestinationAirportUpdatedEvent));
        }

        [Fact]
        public void ChangeDestinationAirport_InvalidDestinationAirportId_ThrowsArgumentNullException()
        {
            // Arrange
            var newDestinationAirportId = Guid.Empty;
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            Action action = () => flight.ChangeDestinationAirport(newDestinationAirportId);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ChangeDestinationAirport_SameAirportAsDeparture_ThrowsArgumentException()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();

            // Act
            Action action = () => flight.ChangeDestinationAirport(flight.DepartureAirportId);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ChangeDestinationAirport_SameDestinationAirportId_ReturnWithoutMakingChanges()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var sameDestinationAirportId = flight.DestinationAirportId;

            // Act
            flight.ChangeDestinationAirport(sameDestinationAirportId);

            // Assert
            flight.DestinationAirportId.Should().Be(sameDestinationAirportId);
            flight.DomainEvents.Should()
                .NotContain(x => x.GetType() == typeof(FlightDestinationAirportUpdatedEvent));
        }

        [Fact]
        public void GetFlightDuration_ValidParameters_Success()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var distance = _fixture.Create<Distance>();

            var expectedDuration = TimeSpan.FromMinutes(flight.TakeOffDurationInMinutes)
                + TimeSpan.FromHours(distance.Kilometers / flight.AverageSpeedInKilometersPerHour);

            // Act
            var duration = flight.GetFlightDuration(distance);

            // Assert
            duration.Should().Be(expectedDuration);
        }

        [Fact]
        public void GetFlightDuration_InvalidDistance_ThrowsArgumentException()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var invalidDistance = new Distance(0);

            // Act
            Action action = () => flight.GetFlightDuration(invalidDistance);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetFuelRequired_ValidParameters_Success()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var distance = _fixture.Create<Distance>();

            var expectedCruiseDuration = TimeSpan.FromHours(distance.Kilometers
                                                            / flight.AverageSpeedInKilometersPerHour);

            var fuelConsumptionInCruise = flight.FuelConsumptionInLitresPerMinute
                                          * expectedCruiseDuration.TotalMinutes;

            var expectedFuelRequired = Volume.FromLitres(flight.TakeOffFuelConsumptionInLitres
                                                   + fuelConsumptionInCruise);

            // Act
            var fuelRequired = flight.GetFuelRequired(distance);

            // Assert
            fuelRequired.Should().Be(expectedFuelRequired);
        }

        [Fact]
        public void GetFuelRequired_InvalidDistance_ThrowsArgumentException()
        {
            // Arrange
            var flight = DomainHelpers.CreateValidFlight();
            var invalidDistance = new Distance(0);

            // Act
            Action action = () => flight.GetFuelRequired(invalidDistance);

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}