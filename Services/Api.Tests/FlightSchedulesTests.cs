using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TravelRepublic.Services.Api;
using TravelRepublic.Services.FlightsService;
using TravelRepublic.Models;

namespace TravelRepublic.Services.Api.Tests
{
  [TestClass()]
  public class FlightSchedulesTests
  {

    private Mock<IFlightBuilder> _flightBuilderMock = new Mock<IFlightBuilder>();

    [TestInitialize]
    public void TestInit()
    {
      var segments1 = new List<Segment>() 
      {
        {new Segment() { DepartureDate = new DateTime(2016,3,1,11,0,0), ArrivalDate = new DateTime(2016,3,1,13,0,0)}},
        {new Segment() { DepartureDate = new DateTime(2016,3,1,18,0,0), ArrivalDate = new DateTime(2016,3,1,20,0,0)}}
      };
      var flight1 = new Flight() { Segments = segments1 };

      var segments2 = new List<Segment>() 
      {
        {new Segment() { DepartureDate = new DateTime(2016,3,2,10,0,0), ArrivalDate = new DateTime(2016,3,2,15,0,0)}},
        {new Segment() { DepartureDate = new DateTime(2016,3,2,14,0,0), ArrivalDate = new DateTime(2016,3,2,19,0,0)}}
      };
      var flight2 = new Flight() { Segments = segments2 };

      var segments3 = new List<Segment>() 
      {
        {new Segment() { DepartureDate = new DateTime(2016,3,3,14,0,0), ArrivalDate = new DateTime(2016,3,3,12,0,0)}}
      };
      var flight3 = new Flight() { Segments = segments3 };

      var flight4 = new Flight() { Segments = new List<Segment>() { new Segment() } };

      var flight5 = new Flight() { Segments = new List<Segment>() };

      var flight6 = new Flight();

      var flights = new List<Flight>() { flight1, flight2, flight3, flight4, flight5, flight6 };
      _flightBuilderMock.Setup(fb => fb.GetFlights()).Returns(flights);
    }

    [TestMethod()]
    public void GetFlightsDepartingBeforeTest()
    {
      var flightSchedules = new FlightSchedules(_flightBuilderMock.Object);
      var flights = flightSchedules.GetFlightsDepartingBefore(new DateTime(2016, 3, 1, 14, 30, 0));
      Assert.AreEqual(1, flights.Count());
      Assert.AreEqual(new DateTime(2016, 3, 1, 11, 0, 0), flights.First().Segments.First().DepartureDate);
    }

    [TestMethod()]
    public void HaveArrivalBeforeDepartureTest()
    {
      var flightSchedules = new FlightSchedules(_flightBuilderMock.Object);
      var flights = flightSchedules.HaveArrivalBeforeDeparture();
      Assert.AreEqual(1, flights.Count());
    }

    [TestMethod()]
    public void SpentMoreThanXTimeOnGroundTest()
    {
      var flightSchedules = new FlightSchedules(_flightBuilderMock.Object);
      var flights = flightSchedules.SpentMoreThanXTimeOnGround(new TimeSpan(2,0,0));
      Assert.AreEqual(1, flights.Count());
    }
  }
}
