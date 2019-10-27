using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FlightManager.Application.Queries;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.TestHelpers;
using FlightManager.TestHelpers.Fixtures;
using FluentAssertions;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.Queries
{
    public class AirportAllTests : IClassFixture<AutoMapperFixture>
    {
        private readonly Mock<IAirportRepository> _airportRepositoryMock;
        private readonly IMapper _mapper;

        public AirportAllTests(AutoMapperFixture autoMapperFixture)
        {
            _airportRepositoryMock = new Mock<IAirportRepository>();
            _mapper = autoMapperFixture.AutoMapper;
        }

        [Fact]
        public async Task AirportAll_ValidQuery_Success()
        {
            // Arrange
            var query = new AirportAll.Query();
            var handler = new AirportAll.Handler(_mapper, _airportRepositoryMock.Object);

            var expectedAirports = DomainHelpers.CreateManyValidAirports().ToList();

            _airportRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedAirports);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Count().Should().Be(expectedAirports.Count);
        }
    }
}