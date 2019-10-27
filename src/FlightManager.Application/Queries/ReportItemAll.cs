using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FlightManager.Application.Dtos;
using FlightManager.Domain.Contracts.Repositories;
using MediatR;

namespace FlightManager.Application.Queries
{
    public class ReportItemAll
    {
        public class Query : IRequest<IEnumerable<ReportItem>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<ReportItem>>
        {
            private readonly IMapper _mapper;
            private readonly IReportItemRepository _reportItemRepository;

            public Handler(IMapper mapper, IReportItemRepository reportItemRepository)
            {
                _mapper = mapper;
                _reportItemRepository = reportItemRepository;
            }

            public async Task<IEnumerable<ReportItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var reportItems = await _reportItemRepository.GetAllAsync();

                return _mapper.Map<IEnumerable<ReportItem>>(reportItems);
            }
        }
    }
}