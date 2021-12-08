using Airports_Client.ViewModels;
using Airports_Logic.Services.FlightsService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airports_Client.Controllers
{
    public class HomeController : Controller
    {
        private IFlightService _flightService;

        public HomeController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(FlightSearchViewModel viewModel)
        {
            FlightSearchArguments searchArguments = new FlightSearchArguments()
            {
                Departure = viewModel.DepartureTime,
                ArrivalAirportId = viewModel.ArrivalAirportId,
                DepartureAirportId = viewModel.DepartureAirportId,
                IsReturnFlight = viewModel.IsReturnFlight,
                NumberOfAdults = viewModel.NumberOfAdults,
                NumberOfChildren = viewModel.NumberOfChildren,
                NumberOfInfants = viewModel.NumberOfInfants,
                Arrival = viewModel.ArrivalTime
            };

            var results = _flightService.SearchFlights(searchArguments);

            viewModel.FlightsSearchResult = results;

            return View(viewModel);
        }
    }
}
