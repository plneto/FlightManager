using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Shared;

namespace FlightManager.Application.Extensions
{
    public static class FlightExtensions
    {
        public static Dtos.Flight ToDto(this Flight flight, Distance distance)
        {
            return new Dtos.Flight
            {
                Id = flight.Id,
                DepartureAirportId = flight.DepartureAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                DistanceInKilometers = distance.Kilometers,
                FuelRequiredInLitres = flight.GetFuelRequired(distance).Litres,
                FlightDuration = flight.GetFlightDuration(distance),
                CreatedAt = flight.CreatedAt
            };
        }
    }
}