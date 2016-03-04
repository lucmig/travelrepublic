using System;
using System.Collections.Generic;

namespace TravelRepublic.Services.Api
{
    using Models;
    public interface IFlightSchedules
    {
        IEnumerable<Flight> GetFlightsDepartingBefore(DateTime dt);
        IEnumerable<Flight> HaveArrivalBeforeDeparture();
        IEnumerable<Flight> SpentMoreThanXTimeOnGround(TimeSpan ts);
    }
}
