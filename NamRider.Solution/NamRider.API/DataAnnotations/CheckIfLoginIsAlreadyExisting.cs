using NamRider.API.NamRiderDBAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.DataAnnotations
{
    /// <summary>
    /// Checking if login of user  entrying is not existing 
    /// </summary>
    public class CheckIfLoginIsAlreadyExisting : ValidationAttribute
    {
        private UserDBMethod _userDBMethod;

        public CheckIfLoginIsAlreadyExisting() : base(Resources.Resources.ExistingUserLogin)
        {
            _userDBMethod = new UserDBMethod();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var validationResult = ValidationResult.Success;
            var login = (string)value;
            try
            {
                var info = _userDBMethod.FindByUsername(login);
                if (info == null)
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