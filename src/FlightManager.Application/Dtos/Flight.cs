using System;

namespace FlightManager.Application.Dtos
{
    public class Flight
    {
        public Guid Id { get; set; }

        public Guid DepartureAirportId { get; set; }

        public Guid DestinationAirportId { get; set; }

        public double DistanceInKilometers { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public double FuelRequiredInLitres { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}