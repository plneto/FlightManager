using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;

namespace FlightManager.Domain.Contracts.Repositories
{
    public interface IReportItemRepository
    {
        Task<IEnumerable<ReportItem>> GetAllAsync();

        Task AddAsync(ReportItem reportItem);
    }
}