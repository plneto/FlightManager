using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using MediatR;
using Serilog;

namespace FlightManager.Application.DomainEventHandlers
{
    public class FlightDestinationAirportUpdatedEventHandler : INotificationHandler<FlightDestinationAirportUpdatedEvent>
    {
        private readonly IReportItemRepository _reportItemRepository;

        public FlightDestinationAirportUpdatedEventHandler(IReportItemRepository reportItemRepository)
        {
            _reportItemRepository = reportItemRepository;
        }

        public async Task Handle(FlightDestinationAirportUpdatedEvent notification, CancellationToken cancellationToken)
        {
            Log.Information($"Entered event handler {nameof(FlightDestinationAirportUpdatedEventHandler)}");

            var report = new ReportItem(nameof(FlightDestinationAirportUpdatedEvent), new
            {
                notification.FlightId,
                notification.NewDestinationAirportId
            });

            await _reportItemRepository.AddAsync(report);
        }
    }
}