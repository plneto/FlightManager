using System;
using FlightManager.Domain.Events;
using FlightManager.Domain.Infrastructure;
using FlightManager.Domain.Shared;

namespace FlightManager.Domain.AggregatesModel.FlightAggregate
{
    public class Flight : Entity, IAggregateRoot
    {
        public Flight(Guid departureAirportId, Guid destinationAirportId)
        {
            if (departureAirportId == default)
            {
                throw new ArgumentNullException(nameof(departureAirportId));
            }

            if (destinationAirportId == default)
            {
                throw new ArgumentNullException(nameof(destinationAirportId));
            }

            if (departureAirportId == destinationAirportId)
            {
                throw new ArgumentException("The departure and destination airports cannot be the same.");
            }

            Id = Guid.NewGuid();
            DepartureAirportId = departureAirportId;
            DestinationAirportId = destinationAirportId;
            CreatedAt = DateTimeOffset.Now;

            AddDomainEvent(new FlightCreatedEvent(Id));
        }

        public Guid DepartureAirportId { get; private set; }

        public Guid DestinationAirportId { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public double TakeOffDurationInMinutes => 30;

        public double TakeOffFuelConsumptionInLitres => 2300;

        public double AverageSpeedInKilometersPerHour => 740;

        public double FuelConsumptionInLitresPerMinute => 240;

        public void ChangeDepartureAirport(Guid newDepartureAirportId)
        {
            if (newDepartureAirportId == default)
            {
                throw new ArgumentNullException(nameof(newDepartureAirportId));
            }

            if (newDepartureAirportId == DestinationAirportId)
            {
                throw new ArgumentException("The new departure airport cannot be the same as the destination airport");
            }

            if (DepartureAirportId == newDepartureAirportId)
            {
                return;
            }

            DepartureAirportId = newDepartureAirportId;

            AddDomainEvent(new FlightDepartureAirportUpdatedEvent(Id, newDepartureAirportId));
        }

        public void ChangeDestinationAirport(Guid newDestinationAirportId)
        {
            if (newDestinationAirportId == default)
            {
                throw new ArgumentNullException(nameof(newDestinationAirportId));
            }

            if (newDestinationAirportId == DepartureAirportId)
            {
                throw new ArgumentException("The new destination airport cannot be the same as the departure airport");
            }

            if (DestinationAirportId == newDestinationAirportId)
            {
                return;
            }

            DestinationAirportId = newDestinationAirportId;

            AddDomainEvent(new FlightDestinationAirportUpdatedEvent(Id, newDestinationAirportId));
        }

        public TimeSpan GetFlightDuration(Distance distance)
        {
            if (distance.Meters <= 0)
            {
                throw new ArgumentException(nameof(distance));
            }

            return TimeSpan.FromMinutes(TakeOffDurationInMinutes)
                   + TimeSpan.FromHours(distance.Kilometers / AverageSpeedInKilometersPerHour);
        }

        public Volume GetFuelRequired(Distance distance)
        {
            if (distance.Meters <= 0)
            {
                throw new ArgumentException(nameof(distance));
            }

            var cruiseDuration = GetFlightDuration(distance) - TimeSpan.FromMinutes(TakeOffDurationInMinutes);

            var fuelConsumptionInCruise = FuelConsumptionInLitresPerMinute * cruiseDuration.TotalMinutes;

            return Volume.FromLitres(TakeOffFuelConsumptionInLitres + fuelConsumptionInCruise);
        }
    }
}