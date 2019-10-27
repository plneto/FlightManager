using System;
using System.Threading;
using System.Threading.Tasks;
using FlightManager.Application.Dtos;
using FlightManager.Application.Extensions;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Contracts.Services;
using MediatR;

namespace FlightManager.Application.Queries
{
    public class FlightById
    {
        public class Query : IRequest<Flight>
        {
            public Query(Guid id)
            {
                if (id == default)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                Id = id;
            }

            public Guid Id { get; }
        }

        public class Handler : IRequestHandler<Query, Flight>
        {
            private readonly IFlightRepository _flightRepository;
            private readonly IFlightDomainService _flightDomainService;

            public Handler(
                IFlightRepository flightRepository,
                IFlightDomainService flightDomainService)
            {
                _flightRepository = flightRepository;
                _flightDomainService = flightDomainService;
            }

            public async Task<Flight> Handle(Query request, CancellationToken cancellationToken)
            {
                var flight = await _flightRepository.GetByIdAsync(request.Id);

                var distance = await _flightDomainService.GetDistanceBetweenAirportsAsync(flight);

                return flight.ToDto(distance);
            }
        }
    }
}