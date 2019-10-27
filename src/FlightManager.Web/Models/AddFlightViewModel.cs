using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlightManager.Web.Models
{
    public class AddFlightViewModel
    {
        public AddFlightViewModel()
        {
            Airports = new List<SelectListItem>();
        }

        public List<SelectListItem> Airports { get; set; }

        public Guid DepartureAirportId { get; set; }

        public Guid DestinationAirportId { get; set; }
    }
}