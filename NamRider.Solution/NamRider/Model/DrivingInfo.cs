using GalaSoft.MvvmLight;
using NamRider.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace NamRider.Model
{
    public partial class DrivingInfoModel //: ObservableObject
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? ValuePertinence { get; set; }
        public bool IsValidatedPertinence { get; set; }
        public bool IsReportedOutDated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LatitudeString { get; set; }
        public string LongitudeString { get; set; }
        public string StreetName { get; set; }
        public string Description { get; set; }
        public int? Severity { get; set; }
        public string AdditionalInfo { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }

        public Geopoint Location { get { return new Geopoint(new BasicGeoposition() { Latitude = Latitude, Longitude = Longitude }); } }

    }
    public class DrivingInfoInputModel
    {
        [Required]
        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        [Required]
        [StringLength(200,MinimumLength = 2)]
        public string StreetName { get; set; }

        [Required]
        [StringLength(200,MinimumLength = 2)]
        public string Description { get; set; }

        public string AdditionalInfo { get; set; }
    }

    public class DrivingInfoEditModel
    {
        [Required]
        public bool IsReported { get; set; }
    }
}
