using System;

namespace FlightManager.Web.Models
{
    public class FlightViewModel
    {
        public Guid Id { get; set; }

        public string DepartureAirport { get; set; }

        public string DestinationAirport { get; set; }

        public double DistanceInKilometers { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public double FuelRequiredInLitres { get; set; }
    }
}