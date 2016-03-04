using System;
using System.Collections.Generic;

namespace TravelRepublic.Services.FlightsService
{
    using Models;
    public interface IFlightBuilder
    {
        IList<Flight> GetFlights();
    }
}
