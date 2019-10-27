using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Shared;

namespace FlightManager.Domain.Contracts.Services
{
    public interface IFlightDomainService
    {
        Task<Distance> GetDistanceBetweenAirportsAsync(Flight flight);
    }
}