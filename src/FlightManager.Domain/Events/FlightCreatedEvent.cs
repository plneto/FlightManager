using System;
using MediatR;

namespace FlightManager.Domain.Events
{
    public class FlightCreatedEvent : INotification
    {
        public FlightCreatedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}