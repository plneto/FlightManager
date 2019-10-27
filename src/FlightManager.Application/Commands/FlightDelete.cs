using System;
using System.Threading;
using System.Threading.Tasks;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using FlightManager.Domain.Extensions;
using MediatR;

namespace FlightManager.Application.Commands
{
    public class FlightDelete
    {
        public class Command : IRequest
        {
            public Command(Guid id)
            {
                if (id == default)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                Id = id;
            }

            public Guid Id { get; }
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
                flight.AddDomainEvent(new FlightDeletedEvent(request.Id));

                await _flightRepository.DeleteAsync(request.Id);
                await _mediator.DispatchDomainEventsAsync(flight);

                return Unit.Value;
            }
        }
    }
}