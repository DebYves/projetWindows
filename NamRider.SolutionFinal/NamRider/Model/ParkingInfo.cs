using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace NamRider.Model
{
    public class ParkingInfoModel
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LatitudeString { get; set; }
        public string LongitudeString { get; set; }
        public string Type { get; set; }
        public double Rayon { get; set; }
        public int? NumberPlace { get; set; }
        public bool? IsEstimatedPlace { get; set; }
        public int? ValuePertinence { get; set; }
        public bool IsValidatedPertinence { get; set; }
        public bool IsReportedOutDated { get; set; }
        public string Description { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }

        //Code qui ne fait pas partie de l'objet de base (celui de l'api)
        //mais qui permet de simplifier son utilisation
        private Geopoint _location = null;
        public Geopoint Location {
            get {
                if(_location == null)
                {
                    _location = new Geopoint(new BasicGeoposition() { Latitude = Latitude, Longitude = Longitude });
                }
                return _location;
            }
        }
        private string _nbPlaceAndEstimated = null;
        public string NbPlaceAndEstimated {
            get {
                if (_nbPlaceAndEstimated == null)
                {
                    _nbPlaceAndEstimated = NumberPlace.ToString();
                    if (IsEstimatedPlace == true)//(parkingInfo.IsEstimatedPlace) marche pas car ca peut valloir null 
                    {
                        _nbPlaceAndEstimated += "(estimation)";
                    }
                }
                return _nbPlaceAndEstimated;
            }
        }
    }

    public class ParkingInfoInputModel
    {
        [Required]
        [Range(0, 999)]
        public decimal Rayon { get; set; }

        [Required]
        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180)]

        public decimal Longitude { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Type { get; set; }

        [Required]
        [Range(1, 99999)]
        public int NumberPlace { get; set; }

        [Required]
        public bool IsEstimatedPlace { get; set; }

        public string Description { get; set; }
    }

    public class ParkingInfoEditModel
    {
        public bool IsReportedDate { get; set; }
    }

    public class ParkingInfoFilterModel
    {
        public string SubcriptionZone { get; set; }
        public string DisckZone { get; set; }
        public string FreeZone { get; set; }
        public string PayZone { get; set; }
        public string AlternancyZone { get; set; }
    }

    public class GeographicPointModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int IdParking { get; set; }
    }
}
