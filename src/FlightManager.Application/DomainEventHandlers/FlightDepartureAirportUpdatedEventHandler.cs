using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using MediatR;
using Serilog;

namespace FlightManager.Application.DomainEventHandlers
{
    public class FlightDepartureAirportUpdatedEventHandler : INotificationHandler<FlightDepartureAirportUpdatedEvent>
    {
        private readonly IReportItemRepository _reportItemRepository;

        public FlightDepartureAirportUpdatedEventHandler(IReportItemRepository reportItemRepository)
        {
            _reportItemRepository = reportItemRepository;
        }

        public async Task Handle(FlightDepartureAirportUpdatedEvent notification, CancellationToken cancellationToken)
        {
            Log.Information($"Entered event handler {nameof(FlightDepartureAirportUpdatedEventHandler)}");

            var report = new ReportItem(nameof(FlightDepartureAirportUpdatedEvent), new
            {
                notification.FlightId,
                notification.NewDepartureAirportId
            });

            await _reportItemRepository.AddAsync(report);
        }
    }
}