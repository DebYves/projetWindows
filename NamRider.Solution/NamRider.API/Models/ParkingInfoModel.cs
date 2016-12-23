using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using NamRider.API.DataAnnotations;

namespace NamRider.API.Models
{
    /// <summary>
    /// ParkingInfo out data class Model
    /// </summary>
    public class ParkingInfoModel
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LatitudeString { get; set; }
        public string LongitudeString { get; set; }
        public string Type { get; set; }
        public decimal Rayon { get; set; }
        public int? NumberPlace { get; set; }
        public bool? IsEstimatedPlace { get; set; }
        public int? ValuePertinence { get; set; }
        public bool IsValidatedPertinence { get; set; }
        public bool IsReportedOutDated { get; set; }
        public string Description { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
    }

    /// <summary>
    /// ParkingInfo input data class Model
    /// </summary>
    public class ParkingInfoInputModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredRayon")]
        [Range(0, 999, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidRayon")]
        public decimal Rayon { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredLatitude")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidLatitude")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredLongitude")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidLongitude")]
        public decimal Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredType")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidType", MinimumLength = 2)]
        public string Type { get; set; }

        [Range(1, 99999, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidNumberPlace")]
        public int? NumberPlace { get; set; }

        public bool? IsEstimatedPlace { get; set; }

        public string Description { get; set; }

    }

    /// <summary>
    /// ParkingInfo reportedDate input data class Model
    /// </summary>
    public class ParkingInfoEditModel
    {
        public bool IsReportedDate { get; set; }
    }

    /// <summary>
    /// ParkingInfo filter type input data class Model
    /// </summary>
    public class ParkingInfoFilterModel
    {
        public string SubcriptionZone { get; set; }
        public string DisckZone { get; set; }
        public string FreeZone { get; set; }
        public string PayZone { get; set; }
        public string AlternancyZone { get; set; }
    }
}