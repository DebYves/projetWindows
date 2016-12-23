using NamRider.API.NamRiderDBAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.DataAnnotations
{
    /// <summary>
    /// Checking if login of user entrying existing 
    /// </summary>
    public class CheckIfLoginExisting : ValidationAttribute
    {
        private UserDBMethod _userDBMethod;
        public CheckIfLoginExisting() : base(Resources.Resources.InvalidLogin)
        {
            _userDBMethod = new UserDBMethod();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var validationResult = ValidationResult.Success;
            var login = (string)value;
            try
            {
                var user = _userDBMethod.FindByUsername(login);
                if (user != null)
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