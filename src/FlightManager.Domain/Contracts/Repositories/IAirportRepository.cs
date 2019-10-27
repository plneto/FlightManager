using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.AirportAggregate;

namespace FlightManager.Domain.Contracts.Repositories
{
    public interface IAirportRepository
    {
        Task<IEnumerable<Airport>> GetAllAsync();

        Task<Airport> GetByIdAsync(Guid id);
    }
}