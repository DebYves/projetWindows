using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRiderAPI.Persistence
{
    public class ParkingInfoFilter
    {
        public string SubcriptionZone { get; set; }
        public string DisckZone { get; set; }
        public string FreeZone { get; set; }
        public string PayZone { get; set; }
        public string AlternancyZone { get; set; }

        public ParkingInfoFilter(string alternancyZone, string disckZone, string freeZone, string payZone, string subcriptionZone)
        {
            AlternancyZone = alternancyZone;
            DisckZone = disckZone;
            FreeZone = freeZone;
            PayZone = payZone;
            SubcriptionZone = subcriptionZone;
        }
    }
}