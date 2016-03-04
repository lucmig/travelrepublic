using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using TravelRepublic.Services.FlightsService;
using TravelRepublic.Services.Api;
using TravelRepublic.Models;

namespace ScheduleApp
{
  class Program
  {
    private CompositionContainer _compositionContainer;

    [Import]
    private IFlightSchedules _schedules;

    private Program()
    {
      var catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(IFlightSchedules).Assembly));
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(IFlightBuilder).Assembly));
      _compositionContainer = new CompositionContainer(catalog);
      _compositionContainer.SatisfyImportsOnce(this);

      var flightsDeparted = _schedules.GetFlightsDepartingBefore(DateTime.Now);
      Console.WriteLine("Departed flights:");
      flightsDeparted.ToList().ForEach(flight =>
        flight.Segments
          .Where(seq => seq.DepartureDate < DateTime.Now).ToList()
          .ForEach(seg => Console.WriteLine(string.Format("\tDeparted: {0}", seg.DepartureDate.ToString("yyyy-MM-dd HH:mm:ss")))));
      Console.WriteLine();

      var arrivalBeforeDeparture = _schedules.HaveArrivalBeforeDeparture();
      Console.WriteLine("Arrivals before departure:");
      arrivalBeforeDeparture.ToList().ForEach(flight =>
        flight.Segments.ToList()
          .ForEach(seg => 
          {
            Console.Write(string.Format("\tArrived: {0}", seg.ArrivalDate.ToString("yyyy-MM-dd HH:mm:ss")));
            Console.WriteLine(string.Format("\tDeparted: {0}", seg.DepartureDate.ToString("yyyy-MM-dd HH:mm:ss")));
          }));
      Console.WriteLine();

      var spentMoreThat2Hours = _schedules.SpentMoreThanXTimeOnGround(new TimeSpan(2,0,0));
      Console.WriteLine("Spent more than two hours on the ground:");
      spentMoreThat2Hours.ToList().ForEach(flight => {
        flight.Segments.ToList()
          .ForEach(seg =>
          {
            Console.WriteLine(string.Format("\tDeparted: {0}", seg.DepartureDate.ToString("yyyy-MM-dd HH:mm:ss")));
            Console.Write(string.Format("\tArrived: {0}", seg.ArrivalDate.ToString("yyyy-MM-dd HH:mm:ss")));
          });
         Console.WriteLine();
         Console.WriteLine();
      });
      Console.WriteLine();
    }
    static void Main(string[] args)
    {
      Program program = new Program();
      Console.Write("Press any key to exit.");
      Console.ReadKey();
    }
  }
}
