using System;

namespace FlightManager.Application.Dtos
{
    public class ReportItem
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public object MessageData { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}