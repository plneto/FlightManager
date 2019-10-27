using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Contracts.Services;
using FlightManager.Domain.Shared;

namespace FlightManager.Domain.Services
{
    public class FlightDomainService : IFlightDomainService
    {
        private readonly IAirportRepository _airportRepository;

        public FlightDomainService(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        public async Task<Distance> GetDistanceBetweenAirportsAsync(Flight flight)
        {
            var departureAirport = await _airportRepository.GetByIdAsync(flight.DepartureAirportId);
            var destinationAirport = await _airportRepository.GetByIdAsync(flight.DestinationAirportId);

            return departureAirport.GetDistanceTo(destinationAirport);
        }
    }
}