using System;
using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Extensions;
using MediatR;

namespace FlightManager.Application.Commands
{
    public class FlightUpdate
    {
        public class Command : IRequest
        {
            public Command(Guid id, Guid departureAirportId, Guid destinationAirportId)
            {
                if (id == default)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                if (departureAirportId == default)
                {
                    throw new ArgumentNullException(nameof(departureAirportId));
                }

                if (destinationAirportId == default)
                {
                    throw new ArgumentNullException(nameof(destinationAirportId));
                }

                Id = id;
                DepartureAirportId = departureAirportId;
                DestinationAirportId = destinationAirportId;
            }

            public Guid Id { get; }

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
                var flight = await _flightRepository.GetByIdAsync(request.Id);

                flight.ChangeDepartureAirport(request.DepartureAirportId);
                flight.ChangeDestinationAirport(request.DestinationAirportId);

                await _flightRepository.UpdateAsync(flight.Id, flight);
                await _mediator.DispatchDomainEventsAsync(flight);

                return Unit.Value;
            }
        }
    }
}