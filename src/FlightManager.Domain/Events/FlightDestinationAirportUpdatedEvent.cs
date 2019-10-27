using System;
using MediatR;

namespace FlightManager.Domain.Events
{
    public class FlightDestinationAirportUpdatedEvent : INotification
    {
        public FlightDestinationAirportUpdatedEvent(Guid flightId, Guid newDestinationAirportId)
        {
            FlightId = flightId;
            NewDestinationAirportId = newDestinationAirportId;
        }

        public Guid FlightId { get; }

        public Guid NewDestinationAirportId { get; }
    }
}