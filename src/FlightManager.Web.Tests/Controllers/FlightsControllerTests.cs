using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.Commands;
using FlightManager.Application.Dtos;
using FlightManager.Application.Queries;
using FlightManager.Web.Constants;
using FlightManager.Web.Controllers;
using FlightManager.Web.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using X.PagedList;
using Xunit;

namespace FlightManager.Web.Tests.Controllers
{
    public class FlightsControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMediator> _mediatorMock;

        public FlightsControllerTests()
        {
            _fixture = new Fixture();
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task IndexGet_ValidRequest_Success()
        {
            // Arrange
            var expectedAirports = _fixture.CreateMany<Airport>();
            var expectedFlights = _fixture.CreateMany<Flight>().ToList();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<AirportAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAirports);

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<FlightAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFlights);

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            var result = await target.Index();

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<ViewResult>().Subject;

            var model = viewResult.ViewData.Model
                .Should()
                .BeOfType<PagedList<FlightViewModel>>().Subject;

            model
                .Should()
                .HaveCount(expectedFlights.Count);
        }

        [Fact]
        public async Task CreateGet_ValidRequest_Success()
        {
            // Arrange
            var expectedAirports = _fixture.CreateMany<Airport>().ToList();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<AirportAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAirports);

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            var result = await target.Create();

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<ViewResult>().Subject;

            var model = viewResult.ViewData.Model
                .Should()
                .BeOfType<AddFlightViewModel>().Subject;

            model.Airports
                .Should()
                .HaveCount(expectedAirports.Count);
        }

        [Fact]
        public async Task CreatePost_ValidRequest_Success()
        {
            // Arrange
            var viewModel = _fixture.Create<AddFlightViewModel>();

            var tempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>())
            {
                [nameof(DefaultMessages.FlightCreatedMessage)] = DefaultMessages.FlightCreatedMessage
            };

            var target = new FlightsController(_mediatorMock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = await target.Create(viewModel);

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<RedirectToActionResult>().Subject;

            viewResult.ActionName
                .Should()
                .Be(nameof(FlightsController.Index));

            _mediatorMock.Verify(x => x.Send(
                It.IsAny<FlightCreate.Command>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task CreatePost_SameAirports_ReturnsError()
        {
            // Arrange
            var airportId = Guid.NewGuid();
            var viewModel = new AddFlightViewModel
            {
                DepartureAirportId = airportId,
                DestinationAirportId = airportId
            };

            var expectedAirports = _fixture.CreateMany<Airport>().ToList();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<AirportAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAirports);

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            var result = await target.Create(viewModel);

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<ViewResult>().Subject;

            var model = viewResult.ViewData.Model
                .Should()
                .BeOfType<AddFlightViewModel>().Subject;

            model.Airports
                .Should()
                .HaveCount(expectedAirports.Count);

            target.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task EditGet_ValidRequest_Success()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();
            var expectedAirports = _fixture.CreateMany<Airport>().ToList();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<AirportAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAirports);

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<FlightById.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(flight);

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            var result = await target.Edit(flight.Id);

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<ViewResult>().Subject;

            var model = viewResult.ViewData.Model
                .Should()
                .BeOfType<EditFlightViewModel>().Subject;

            model.Airports
                .Should()
                .HaveCount(expectedAirports.Count);

            model.FlightId.Should().Be(flight.Id);
        }

        [Fact]
        public void EditGet_EmptyId_ThrowsArgumentNullException()
        {
            // Arrange
            var id = Guid.Empty;

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            Func<Task> action = async () => await target.Edit(id);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task EditPost_ValidRequest_Success()
        {
            // Arrange
            var viewModel = _fixture.Create<EditFlightViewModel>();

            var tempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>())
            {
                [nameof(DefaultMessages.FlightUpdatedMessage)] = DefaultMessages.FlightUpdatedMessage
            };

            var target = new FlightsController(_mediatorMock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = await target.Edit(viewModel);

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<RedirectToActionResult>().Subject;

            viewResult.ActionName
                .Should()
                .Be(nameof(FlightsController.Index));

            _mediatorMock.Verify(x => x.Send(
                It.IsAny<FlightUpdate.Command>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task EditPost_SameAirports_ReturnsError()
        {
            // Arrange
            var flightId = Guid.NewGuid();
            var airportId = Guid.NewGuid();
            var viewModel = new EditFlightViewModel
            {
                FlightId = flightId,
                DepartureAirportId = airportId,
                DestinationAirportId = airportId
            };

            var flight = _fixture.Create<Flight>();
            var expectedAirports = _fixture.CreateMany<Airport>().ToList();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<AirportAll.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAirports);

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<FlightById.Query>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(flight);

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            var result = await target.Edit(viewModel);

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<ViewResult>().Subject;

            var model = viewResult.ViewData.Model
                .Should()
                .BeOfType<EditFlightViewModel>().Subject;

            model.Airports
                .Should()
                .HaveCount(expectedAirports.Count);

            target.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Delete_ValidRequest_Success()
        {
            // Arrange
            var flightId = _fixture.Create<Guid>();

            var tempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>())
            {
                [nameof(DefaultMessages.FlightDeletedMessage)] = DefaultMessages.FlightDeletedMessage
            };

            var target = new FlightsController(_mediatorMock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = await target.Delete(flightId);

            // Assert
            var viewResult = result
                .Should()
                .BeOfType<RedirectToActionResult>().Subject;

            viewResult.ActionName
                .Should()
                .Be(nameof(FlightsController.Index));

            _mediatorMock.Verify(x => x.Send(
                It.IsAny<FlightDelete.Command>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void Delete_EmptyId_ThrowsArgumentNullException()
        {
            // Arrange
            var id = Guid.Empty;

            var target = new FlightsController(_mediatorMock.Object);

            // Act
            Func<Task> action = async () => await target.Delete(id);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}