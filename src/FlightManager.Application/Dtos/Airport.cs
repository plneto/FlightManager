using System;

namespace FlightManager.Application.Dtos
{
    public class Airport
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public Coordinates Coordinates { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}