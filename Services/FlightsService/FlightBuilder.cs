using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace TravelRepublic.Services.FlightsService
{
  using Models;

  [Export(typeof(IFlightBuilder))]
  public class FlightBuilder : IFlightBuilder
  {
    #region Private fields

    private DateTime _threeDaysFromNow;

    #endregion

    #region Constructor

    public FlightBuilder()
    {
      _threeDaysFromNow = DateTime.Now.AddDays(3);
    }

    #endregion

    #region IFlightBuilder Members

    public IList<Flight> GetFlights()
    {
      return new List<Flight>
			           {
                     //A normal flight with two hour duration
			               CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2)),

                           //A normal multi segment flight
			               CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(3), _threeDaysFromNow.AddHours(5)),
                           
                           //A flight departing in the past
                           CreateFlight(_threeDaysFromNow.AddDays(-6), _threeDaysFromNow),

                           //A flight that departs before it arrives
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(-6)),

                           //A flight with more than two hours ground time
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(5), _threeDaysFromNow.AddHours(6)),

                            //Another flight with more than two hours ground time
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(3), _threeDaysFromNow.AddHours(4), _threeDaysFromNow.AddHours(6), _threeDaysFromNow.AddHours(7))
			           };
    }

    #endregion

    #region Private Methods

    private static Flight CreateFlight(params DateTime[] dates)
    {
      if (dates.Length % 2 != 0) throw new ArgumentException("You must pass an even number of dates,", "dates");

      var departureDates = dates.Where((date, index) => index % 2 == 0);
      var arrivalDates = dates.Where((date, index) => index % 2 == 1);

      var segments = departureDates.Zip(arrivalDates,
                                        (departureDate, arrivalDate) =>
                                        new Segment { DepartureDate = departureDate, ArrivalDate = arrivalDate }).ToList();

      return new Flight { Segments = segments };
    }

    #endregion

  }
}

