using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FlightManager.Application.Dtos;
using FlightManager.Domain.Contracts.Repositories;
using MediatR;

namespace FlightManager.Application.Queries
{
    public class AirportAll
    {
        public class Query : IRequest<IEnumerable<Airport>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Airport>>
        {
            private readonly IMapper _mapper;
            private readonly IAirportRepository _airportRepository;

            public Handler(IMapper mapper, IAirportRepository airportRepository)
            {
                _mapper = mapper;
                _airportRepository = airportRepository;
            }

            public async Task<IEnumerable<Airport>> Handle(Query request, CancellationToken cancellationToken)
            {
                var airports = await _airportRepository.GetAllAsync();

                return _mapper.Map<IEnumerable<Airport>>(airports);
            }
        }
    }
}