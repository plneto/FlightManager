using System;

namespace FlightManager.Web.Models
{
    public class EditFlightViewModel : AddFlightViewModel
    {
        public Guid FlightId { get; set; }
    }
}