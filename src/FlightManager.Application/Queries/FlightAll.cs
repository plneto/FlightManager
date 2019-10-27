using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FlightManager.Application.Dtos;
using FlightManager.Application.Extensions;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Contracts.Services;
using MediatR;

namespace FlightManager.Application.Queries
{
    public class FlightAll
    {
        public class Query : IRequest<IEnumerable<Flight>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Flight>>
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

            public async Task<IEnumerable<Flight>> Handle(Query request, CancellationToken cancellationToken)
            {
                var flights = await _flightRepository.GetAllAsync();

                var result = new List<Flight>();

                foreach (var flight in flights)
                {
                    var distance = await _flightDomainService.GetDistanceBetweenAirportsAsync(flight);

                    result.Add(flight.ToDto(distance));
                }

                return result;
            }
        }
    }
}