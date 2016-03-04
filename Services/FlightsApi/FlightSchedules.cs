using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace TravelRepublic.Services.Api
{
  using Models;
  using FlightsService;

  [Export(typeof(IFlightSchedules))]
  public class FlightSchedules : IFlightSchedules
  {
    #region Private fields

    private readonly IFlightBuilder _flightBuilder;

    #endregion

    #region Constructor

    [ImportingConstructor]
    public FlightSchedules(IFlightBuilder flightBuilder)
    {
      _flightBuilder = flightBuilder;
    }

    #endregion

    #region IFlightSchedules Members

    public IEnumerable<Flight> GetFlightsDepartingBefore(DateTime dt)
    {
      var flights = _flightBuilder.GetFlights();
      return flights
        .Where(flight => flight.Segments != null && flight.Segments.Count > 0)
        .Where(flight => flight.Segments.Any(seg =>
          seg.ArrivalDate != default(DateTime)
          && seg.DepartureDate != default(DateTime)
          && seg.DepartureDate < dt));
    }

    public IEnumerable<Flight> HaveArrivalBeforeDeparture()
    {
      var flights = _flightBuilder.GetFlights();
      return flights
        .Where(flight => flight.Segments != null && flight.Segments.Count > 0)
        .Where(flight => flight.Segments.Any(seg =>
          seg.ArrivalDate != default(DateTime)
          && seg.DepartureDate != default(DateTime) 
          && seg.ArrivalDate < seg.DepartureDate));
    }

    public IEnumerable<Flight> SpentMoreThanXTimeOnGround(TimeSpan ts) 
        {
          var flights = _flightBuilder.GetFlights();
          return flights
            .Where(flight => flight.Segments != null && flight.Segments.Count > 0)
            .Where(flight => flight.Segments.Any(seg => 
            {
              using (var iter = flight.Segments.GetEnumerator())
              {
                if (iter.MoveNext())
                {
                  var previous = iter.Current;
                  while (iter.MoveNext())
                  {
                    if (previous.DepartureDate != default(DateTime)
                      && previous.ArrivalDate != default(DateTime)
                      && (iter.Current.DepartureDate - previous.ArrivalDate) > ts)
                      return true;
                  }
                }
                return false;
              }
            }));
        }

    #endregion
  }
}
