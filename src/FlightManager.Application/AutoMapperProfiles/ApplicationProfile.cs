using AutoMapper;
using FlightManager.Domain.AggregatesModel.AirportAggregate;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;

namespace FlightManager.Application.AutoMapperProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Airport, Dtos.Airport>();
            CreateMap<Coordinates, Dtos.Coordinates>();
            CreateMap<ReportItem, Dtos.ReportItem>();
        }
    }
}