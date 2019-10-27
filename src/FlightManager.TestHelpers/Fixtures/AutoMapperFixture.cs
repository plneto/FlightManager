using AutoMapper;
using FlightManager.Application.AutoMapperProfiles;

namespace FlightManager.TestHelpers.Fixtures
{
    public class AutoMapperFixture
    {
        public AutoMapperFixture()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });

            AutoMapper = mockMapper.CreateMapper();
        }

        public IMapper AutoMapper { get; set; }
    }
}