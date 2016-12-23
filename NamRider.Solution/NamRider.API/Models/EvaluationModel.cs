using NamRider.API.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.Models
{
    /// <summary>
    /// Evaluation out data class Model
    /// </summary>
    public class EvaluationModel
    {
        public int IdDriving { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Evaluation input data class Model
    /// </summary>
    public class EvaluationInputViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredIdDriving")]
        [CheckIfDrivingExisting]
        public int IdDriving { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RequiredValueEval")]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidValueEval")]
        public int Value { get; set; }
    }
}