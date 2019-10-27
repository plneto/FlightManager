using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using MediatR;
using Serilog;

namespace FlightManager.Application.DomainEventHandlers
{
    public class FlightDeletedEventHandler : INotificationHandler<FlightDeletedEvent>
    {
        private readonly IReportItemRepository _reportItemRepository;

        public FlightDeletedEventHandler(IReportItemRepository reportItemRepository)
        {
            _reportItemRepository = reportItemRepository;
        }

        public async Task Handle(FlightDeletedEvent notification, CancellationToken cancellationToken)
        {
            Log.Information($"Entered event handler {nameof(FlightDeletedEventHandler)}");

            var report = new ReportItem(nameof(FlightDeletedEvent), new
            {
                notification.Id
            });

            await _reportItemRepository.AddAsync(report);
        }
    }
}