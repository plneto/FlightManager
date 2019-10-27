using System.Linq;
using System.Threading.Tasks;
using FlightManager.Application.Queries;
using FlightManager.Web.Constants;
using FlightManager.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FlightManager.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var reportItemsQuery = new ReportItemAll.Query();
            var reportItems = await _mediator.Send(reportItemsQuery);

            var onePageOfReportItems = await reportItems
                .OrderByDescending(x => x.CreatedAt)
                .ToPagedListAsync(page, DefaultSettings.PageSize);

            var viewModel = new ReportViewModel
            {
                ReportItems = onePageOfReportItems
            };

            return View(viewModel);
        }
    }
}