using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.AirportAggregate;
using FlightManager.Domain.Contracts.Repositories;
using MongoDB.Driver;

namespace FlightManager.Data.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly IMongoCollection<Airport> _collection;

        public AirportRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Airport>(nameof(Airport));
        }

        public async Task<Airport> GetByIdAsync(Guid id)
        {
            return await _collection
                .Find(x => x.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Airport>> GetAllAsync()
        {
            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }
    }
}