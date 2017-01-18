using NamRider.API.NamRiderDBAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.DataAnnotations
{
    /// <summary>
    /// Checking if id of driving  entrying existing 
    /// </summary>
    public class CheckIfDrivingExisting : ValidationAttribute
    {
        private DrivingInfoDBMethod _drivingInfoDBMethod;

        public CheckIfDrivingExisting() : base(Resources.Resources.InvalidIdDriving)
        {
            _drivingInfoDBMethod = new DrivingInfoDBMethod();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var validationResult = ValidationResult.Success;
            var id = (int)value;
            try
            {
                var driving = _drivingInfoDBMethod.FindById(id);
                if (driving != null)
                    return validationResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new ValidationResult(ErrorMessageString);
        }
    }

    /// <summary>
    /// Checking if id of park  entrying existing 
    /// </summary>
    public class CheckIfParkingExisting : ValidationAttribute
    {
        private ParkingInfoDBMethod _parkingInfoDBMethod;

        public CheckIfParkingExisting() : base(Resources.Resources.InvalidIdPark)
        {
            _parkingInfoDBMethod = new ParkingInfoDBMethod();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var validationResult = ValidationResult.Success;
            var id = (int)value;
            try
            {
                var parking = _parkingInfoDBMethod.FindById(id);
                if (parking != null)
                    return validationResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new ValidationResult(ErrorMessageString);
        }
    }
}