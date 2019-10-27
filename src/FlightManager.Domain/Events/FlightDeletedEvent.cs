using System;
using MediatR;

namespace FlightManager.Domain.Events
{
    public class FlightDeletedEvent : INotification
    {
        public FlightDeletedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}