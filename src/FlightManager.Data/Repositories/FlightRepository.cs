using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Contracts.Repositories;
using MongoDB.Driver;

namespace FlightManager.Data.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly IMongoCollection<Flight> _collection;

        public FlightRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Flight>(nameof(Flight));
        }

        public async Task<Flight> GetByIdAsync(Guid id)
        {
            return await _collection
                .Find(x => x.Id == id)
                .SingleOrDefaultAsync();
        }

        public Task AddAsync(Flight flight)
        {
            return _collection.InsertOneAsync(flight);
        }

        public Task UpdateAsync(Guid id, Flight flight)
        {
            return _collection.ReplaceOneAsync(x => x.Id == id, flight);
        }

        public Task DeleteAsync(Guid id)
        {
            return _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }
    }
}