using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.Dtos;
using FlightManager.Application.Queries;
using FlightManager.Web.Controllers;
using FlightManager.Web.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightManager.Web.Tests.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMediator> _mediatorMock;

        public ReportsControllerTests()
        {
            _fixture = new Fixture();
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task IndexGet_ValidRequest_Success()
        {
            // Arrange
            var expectedReportItems = _fixture.CreateMany<ReportItem>().ToList();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<ReportItemAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedReportItems);

            var target = new ReportsController(_mediatorMock.Object);

            // Act
            var result = await target.Index();

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<ViewResult>().Subject;

            var model = viewResult.ViewData.Model
                .Should()
                .BeOfType<ReportViewModel>().Subject;

            model.ReportItems
                .Should()
                .HaveCount(expectedReportItems.Count);
        }
    }
}