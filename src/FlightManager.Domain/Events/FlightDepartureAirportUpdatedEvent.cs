using System;
using MediatR;

namespace FlightManager.Domain.Events
{
    public class FlightDepartureAirportUpdatedEvent : INotification
    {
        public FlightDepartureAirportUpdatedEvent(Guid flightId, Guid newDepartureAirportId)
        {
            FlightId = flightId;
            NewDepartureAirportId = newDepartureAirportId;
        }

        public Guid FlightId { get; }

        public Guid NewDepartureAirportId { get; }
    }
}