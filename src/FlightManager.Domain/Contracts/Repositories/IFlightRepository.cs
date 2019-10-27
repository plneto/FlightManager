using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.FlightAggregate;

namespace FlightManager.Domain.Contracts.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllAsync();

        Task<Flight> GetByIdAsync(Guid id);

        Task AddAsync(Flight flight);

        Task UpdateAsync(Guid id, Flight flight);

        Task DeleteAsync(Guid id);
    }
}