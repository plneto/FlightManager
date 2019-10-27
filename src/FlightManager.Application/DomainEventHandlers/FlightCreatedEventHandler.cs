using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using MediatR;
using Serilog;

namespace FlightManager.Application.DomainEventHandlers
{
    public class FlightCreatedEventHandler : INotificationHandler<FlightCreatedEvent>
    {
        private readonly IReportItemRepository _reportItemRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IAirportRepository _airportRepository;

        public FlightCreatedEventHandler(
            IReportItemRepository reportItemRepository,
            IFlightRepository flightRepository,
            IAirportRepository airportRepository)
        {
            _reportItemRepository = reportItemRepository;
            _flightRepository = flightRepository;
            _airportRepository = airportRepository;
        }

        public async Task Handle(FlightCreatedEvent notification, CancellationToken cancellationToken)
        {
            Log.Information($"Entered event handler {nameof(FlightCreatedEventHandler)}");

            var flight = await _flightRepository.GetByIdAsync(notification.Id);

            var departureAirport = await _airportRepository.GetByIdAsync(flight.DepartureAirportId);
            var destinationAirport = await _airportRepository.GetByIdAsync(flight.DestinationAirportId);

            var distance = departureAirport.GetDistanceTo(destinationAirport);

            var report = new ReportItem(nameof(FlightCreatedEvent), new
            {
                flight.Id,
                DepartureAirport = departureAirport.ToString(),
                DestinationAirport = destinationAirport.ToString(),
                FlightDistanceInKilometers = distance.Kilometers,
                FlightDuration = flight.GetFlightDuration(distance),
                FuelRequiredInLitres = flight.GetFuelRequired(distance).Litres,
                flight.CreatedAt
            });

            await _reportItemRepository.AddAsync(report);
        }
    }
}