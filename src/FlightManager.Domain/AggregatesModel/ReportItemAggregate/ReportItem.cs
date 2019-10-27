using System;
using FlightManager.Domain.Infrastructure;

namespace FlightManager.Domain.AggregatesModel.ReportItemAggregate
{
    public class ReportItem : Entity, IAggregateRoot
    {
        public ReportItem(string message, object messageData)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Id = Guid.NewGuid();
            Message = message;
            MessageData = messageData;
            CreatedAt = DateTimeOffset.Now;
        }

        public string Message { get; private set; }

        public object MessageData { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
    }
}