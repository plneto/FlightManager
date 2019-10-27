using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FlightManager.Domain.AggregatesModel.AirportAggregate;
using FlightManager.Domain.AggregatesModel.FlightAggregate;

namespace FlightManager.TestHelpers
{
    public static class DomainHelpers
    {
        public static Flight CreateValidFlight()
        {
            return new Flight(Guid.NewGuid(), Guid.NewGuid());
        }

        public static IEnumerable<Flight> CreateManyValidFlights(int count = 10)
        {
            var result = new List<Flight>();

            for (var i = 0; i < count; i++)
            {
                result.Add(CreateValidFlight());
            }

            return result;
        }

        public static Airport CreateValidAirport()
        {
            var fixture = new Fixture();

            return new Airport(
                fixture.Create<string>(),
                fixture.Create<string>(),
                GetValidLatitude(),
                GetValidLongitude());
        }

        public static IEnumerable<Airport> CreateManyValidAirports(int count = 10)
        {
            var result = new List<Airport>();

            for (var i = 0; i < count; i++)
            {
                result.Add(CreateValidAirport());
            }

            return result;
        }

        public static double GetValidLatitude()
        {
            var fixture = new Fixture();
            var doubleGenerator = new Generator<double>(fixture);

            return doubleGenerator.First(x => x >= -90 && x <= 90);
        }

        public static double GetValidLongitude()
        {
            var fixture = new Fixture();
            var doubleGenerator = new Generator<double>(fixture);

            return doubleGenerator.First(x => x >= -180 && x <= 180);
        }
    }
}