using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FlightManager.Application.Queries;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.TestHelpers.Fixtures;
using FluentAssertions;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.Queries
{
    public class ReportItemAllTests : IClassFixture<AutoMapperFixture>
    {
        private readonly Fixture _fixture;
        private readonly Mock<IReportItemRepository> _reportItemRepositoryMock;
        private readonly IMapper _mapper;

        public ReportItemAllTests(AutoMapperFixture autoMapperFixture)
        {
            _reportItemRepositoryMock = new Mock<IReportItemRepository>();
            _mapper = autoMapperFixture.AutoMapper;
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReportItemAll_ValidQuery_Success()
        {
            // Arrange
            var query = new ReportItemAll.Query();
            var handler = new ReportItemAll.Handler(_mapper, _reportItemRepositoryMock.Object);

            var expectedReportItems = _fixture.CreateMany<ReportItem>().ToList();

            _reportItemRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedReportItems);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Count().Should().Be(expectedReportItems.Count);
        }
    }
}