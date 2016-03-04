using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using TravelRepublic.Services.Api;
using TravelRepublic.Models;

namespace ScheduleApp
{
  public class Sample
  {
    [ImportingConstructor]
    public Sample(IFlightSchedules schedules)
    {

    }
  }
}
