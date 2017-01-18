using NamRider.API.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.Models
{
    /// <summary>
    /// CriticismDriving out data class model
    /// </summary>
    public class CriticismDrivingModel
    {
        public int IdDriving { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    ///  CriticismDriving input data class model
    /// </summary>
    public class CriticismDrivingInputModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredIdDriving")]
        [CheckIfDrivingExisting]
        public int IdDriving { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredValueCritism")]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidValueCritism")]
        public int Value { get; set; }
    }


    /// <summary>
    ///  CriticismParking out data class model
    /// </summary>
    public class CriticismParkingModel
    {
        public int IdParking { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public int Value { get; set; }
    }


    /// <summary>
    /// CriticismParking input data class model
    /// </summary>
    public class CriticismParkingInputModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredIdPark")]
        [CheckIfParkingExisting]
        public int IdParking { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredValueCritism")]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidValueCritism")]
        public int Value { get; set; }
    }
}