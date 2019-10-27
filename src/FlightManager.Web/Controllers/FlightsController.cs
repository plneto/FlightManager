using System;
using System.Linq;
using System.Threading.Tasks;
using FlightManager.Application.Commands;
using FlightManager.Application.Queries;
using FlightManager.Web.Constants;
using FlightManager.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace FlightManager.Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IMediator _mediator;

        public FlightsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var airportsQuery = new AirportAll.Query();
            var airports = (await _mediator.Send(airportsQuery)).ToList();

            var flightsQuery = new FlightAll.Query();
            var flights = await _mediator.Send(flightsQuery);

            var onePageOfFLights = await flights
                .OrderByDescending(x => x.CreatedAt)
                .ToPagedListAsync(page, DefaultSettings.PageSize);

            var viewModels = onePageOfFLights.Select(flight => new FlightViewModel
            {
                Id = flight.Id,
                DepartureAirport = airports.SingleOrDefault(x => x.Id == flight.DepartureAirportId)?.Name,
                DestinationAirport = airports.SingleOrDefault(x => x.Id == flight.DestinationAirportId)?.Name,
                DistanceInKilometers = flight.DistanceInKilometers,
                FuelRequiredInLitres = flight.FuelRequiredInLitres,
                FlightDuration = flight.FlightDuration
            });

            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = await GetAddFlightViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddFlightViewModel model)
        {
            if (model.DepartureAirportId == model.DestinationAirportId)
            {
                ModelState.AddModelError(string.Empty, DefaultMessages.SameAirportErrorMessage);

                var viewModel = await GetAddFlightViewModel();

                return View(viewModel);
            }

            var createFlightCommand = new FlightCreate.Command(
                model.DepartureAirportId,
                model.DestinationAirportId);

            await _mediator.Send(createFlightCommand);

            TempData[nameof(DefaultMessages.FlightCreatedMessage)] = DefaultMessages.FlightCreatedMessage;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            if (id == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var viewModel = await GetEditFlightViewModel(id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFlightViewModel model)
        {
            if (model.DepartureAirportId == model.DestinationAirportId)
            {
                ModelState.AddModelError(string.Empty, DefaultMessages.SameAirportErrorMessage);

                var viewModel = await GetEditFlightViewModel(model.FlightId);

                return View(viewModel);
            }

            var updateFlightCommand = new FlightUpdate.Command(
                model.FlightId,
                model.DepartureAirportId,
                model.DestinationAirportId);

            await _mediator.Send(updateFlightCommand);

            TempData[nameof(DefaultMessages.FlightUpdatedMessage)] = DefaultMessages.FlightUpdatedMessage;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var deleteFlightCommand = new FlightDelete.Command(id);
            await _mediator.Send(deleteFlightCommand);

            TempData[nameof(DefaultMessages.FlightDeletedMessage)] = DefaultMessages.FlightDeletedMessage;

            return RedirectToAction(nameof(Index));
        }

        private async Task<AddFlightViewModel> GetAddFlightViewModel()
        {
            var airportsQuery = new AirportAll.Query();
            var airports = await _mediator.Send(airportsQuery);

            return new AddFlightViewModel
            {
                Airports = airports
                    .Select(x => new SelectListItem($"{x.Name} ({x.Code})", x.Id.ToString()))
                    .ToList()
            };
        }

        private async Task<EditFlightViewModel> GetEditFlightViewModel(Guid flightId)
        {
            var airportsQuery = new AirportAll.Query();
            var airports = await _mediator.Send(airportsQuery);

            var flightByIdQuery = new FlightById.Query(flightId);
            var flight = await _mediator.Send(flightByIdQuery);

            return new EditFlightViewModel
            {
                FlightId = flightId,
                Airports = airports
                    .Select(x => new SelectListItem($"{x.Name} ({x.Code})", x.Id.ToString()))
                    .ToList(),
                DepartureAirportId = flight.DepartureAirportId,
                DestinationAirportId = flight.DestinationAirportId
            };
        }
    }
}