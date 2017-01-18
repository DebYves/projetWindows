using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamRider.API.Models
{
    /// <summary>
    /// GeographicPoint out data class Model
    /// </summary>
    public class GeographicPointModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int IdParking { get; set; }
    }
}