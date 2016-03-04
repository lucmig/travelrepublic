using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelRepublic.Models
{
    public class Flight
    {
        public IList<Segment> Segments { get; set; }
    }
}
