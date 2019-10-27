using System.Collections.Generic;
using FlightManager.Application.Dtos;

namespace FlightManager.Web.Models
{
    public class ReportViewModel
    {
        public IEnumerable<ReportItem> ReportItems { get; set; }
    }
}