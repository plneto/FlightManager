using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using MongoDB.Driver;

namespace FlightManager.Data.Repositories
{
    public class ReportItemRepository : IReportItemRepository
    {
        private readonly IMongoCollection<ReportItem> _collection;

        public ReportItemRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ReportItem>(nameof(ReportItem));
        }

        public async Task<IEnumerable<ReportItem>> GetAllAsync()
        {
            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }

        public Task AddAsync(ReportItem reportItem)
        {
            return _collection.InsertOneAsync(reportItem);
        }
    }
}