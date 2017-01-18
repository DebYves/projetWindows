using NamRider.API.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.Models
{
    /// <summary>
    /// DrivingInfo out data class Model
    /// </summary>
    public class DrivingInfoModel
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int? ValuePertinence { get; set; }
        public bool IsValidatedPertinence { get; set; }
        public bool IsReportedOutDated { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LatitudeString { get; set; }
        public string LongitudeString { get; set; }
        public string StreetName { get; set; }
        public string Description { get; set; }
        public int? Severity { get; set; }
        public string AdditionalInfo { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }

    }

    /// <summary>
    /// DrivingInfo input data class Model
    /// </summary>
    public class DrivingInfoInputModel
    {
        public string AdditionalInfo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredLatitude")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidLatitude")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredLongitude")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidLongitude")]
        public decimal Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredStreetName")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidStreetName", MinimumLength = 2)]
        public string StreetName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredDescription")]
        [StringLength(350, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidDescription", MinimumLength = 2)]
        public string Description { get; set; }

    }

    /// <summary>
    /// DrivingInfo input reportedDate data class Model
    /// </summary>
    public class DrivingInfoEditModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredReportedDate")]
        public bool IsReported { get; set;  }
    }
}