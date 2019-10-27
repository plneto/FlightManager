using System;
using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.AggregatesModel.FlightAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Extensions;
using MediatR;

namespace FlightManager.Application.Commands
{
    public class FlightCreate
    {
        public class Command : IRequest
        {
            public Command(Guid departureAirportId, Guid destinationAirportId)
            {
                if (departureAirportId == default)
                {
                    throw new ArgumentNullException(nameof(departureAirportId));
                }

                if (destinationAirportId == default)
                {
                    throw new ArgumentNullException(nameof(destinationAirportId));
                }

                DepartureAirportId = departureAirportId;
                DestinationAirportId = destinationAirportId;
            }

            public Guid DepartureAirportId { get; }

            public Guid DestinationAirportId { get; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IMediator _mediator;
            private readonly IFlightRepository _flightRepository;

            public Handler(IMediator mediator, IFlightRepository flightRepository)
            {
                _mediator = mediator;
                _flightRepository = flightRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var flight = new Flight(request.DepartureAirportId, request.DestinationAirportId);

                await _flightRepository.AddAsync(flight);
                await _mediator.DispatchDomainEventsAsync(flight);

                return Unit.Value;
            }
        }
    }
}