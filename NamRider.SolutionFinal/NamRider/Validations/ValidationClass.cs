using NamRider.DAO;
using NamRider.Model;
using NamRider.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NamRider.Validations
{
    public class ValidationClass
    {
        private UserService _userService = new UserService();
        private CriticismService _criticismService = new CriticismService();
        private EvaluationService _evaluationService = new EvaluationService();
        private ParkingInfoService _parkingInfoService = new ParkingInfoService();

        public string UserLoginDataValidation(UserLogin userLogin)
        {
            if (userLogin.UserName == null)
                return "Login obligatoire";

            if (userLogin.Password == null)
                return "Mot de passe obligatoire";

            return ApiConstants.OkMessage;
        }

        public string UserRegisterDataValidation(UserInput userRegister)
        {

            if (userRegister.Email == null || userRegister.Email.Length > 256)
                return "Adresse mail obligatoire";
            if (!CheckEmailAddressFormat(userRegister.Email))
                return "Format d'adresse mail incorrect";
            if (CheckIfEmailAddressNotAlreadyExisting(userRegister.Email))
                return "Adresse email déjà existante pour un utilisateur";

            if (userRegister.UserName == null || userRegister.UserName.Length < 6 || userRegister.UserName.Length > 128)
                return "Login obligatoire";
            if (CheckIfExistingUsername(userRegister.UserName))
                return "Login déjà existant pour un utilisateur";

            if ((userRegister.Password == null || userRegister.Password.Length < 6 || userRegister.Password.Length > 50))
                return "Mot de passe obligatoire. Minimum 6 caratères et maximum 50 caratères";
            if (!CheckValidPassoword(userRegister.Password))
                return "Le mot de passe doit contenir au moins une lettre majuscule, un chiffre et un caractère spécial";

            if (userRegister.ConfirmPassword == null || !userRegister.ConfirmPassword.Equals(userRegister.Password))
                return "La confirmation doit être identique au mot de passe";

            return ApiConstants.OkMessage;
        }

        public string DrivingValidation(DrivingInfoInputModel driving)
        {
            if (driving.Latitude.Equals(0) || driving.Latitude < -90 || driving.Latitude > 90)
                return "Latitude obligatoire et comprise entre -90 et 90";

            if (driving.Longitude.Equals(0) || driving.Longitude < -180 || driving.Longitude > 180)
                return "Longitude obligatoire et comprise entre -180 et 180";

            if (driving.StreetName == null)//|| driving.StreetName.Length < 2 || driving.StreetName.Length > 200)
                return "Rue obligatoire.";// Entre 2 et 200 caratères"; //Le street finder bing map ne trouve pas toujours une rue à l'endroit cliqué...

            if (driving.Description == null || driving.Description.Length < 2 || driving.Description.Length > 350)
                return "Description obligatoire!!!! Entre 2 et 350 caratères";

            if (driving.AdditionalInfo != null)
            {
                if (driving.AdditionalInfo.Length < 2 || driving.AdditionalInfo.Length > 300)
                    return "L'information additionnelle est facultative ou entre 2 et 300 caratères";
            }
            return ApiConstants.OkMessage;
        }

        public string ParkingValidation(ParkingInfoInputModel park)
        {
            if (park.Rayon <= 0 || park.Rayon > 999)
                return "Rayon compris entre 1 et 1000 mètres";

            if (park.Type == null )
                return "Type du parking obligatoire!";

            if (park.Latitude.Equals(0) || park.Latitude < -90 || park.Latitude > 90)
                return "Latitude obligatoire et comprise entre -90 et 90";

            if (park.Longitude.Equals(0) || park.Longitude < -180 || park.Longitude > 180)
                return "Longitude obligatoire et comprise entre -180 et 180";

            if (CheckIfParkingExistingAlredy(park.Latitude, park.Longitude))
                return "Un parking avec les mêmes localisations existe déjà";

            if (park.Description != null)
            {
                if (park.Description.Length < 2 || park.Description.Length > 350)
                    return "La description est facultative ou entre 2 et 300 caratères";
            }
            return ApiConstants.OkMessage;
        }

        public string ValidationDrivingCritism(CriticismDrivingInputModel critism)
        {
            var drivingCritism = _criticismService.GetCritismDrivingById(ApiConstants.UserId, critism.IdDriving).Result;
            if (drivingCritism != null)
                return "Désole!!!!! Vous avez déjà fait une critique pour cette info de roulage.";

            if (critism.Value < 1 || critism.Value > 100)
                return "La valeur de la critique est un nombre entre 1 et 100 ";

            return ApiConstants.OkMessage;
        }

        public string ValidationParkCritism(CriticismParkingInputModel critism)
        {
            var parkCritism = _criticismService.GetCritismParkingInfoById(ApiConstants.UserId, critism.IdParking).Result;
            if (parkCritism != null)
                return "Désole!!!!! Vous avez déjà fait une critique pour ce parking.";

            if (critism.Value < 1 || critism.Value > 100)
                return "La valeur de la critique est un nombre entre 1 et 100 ";

            return ApiConstants.OkMessage;
        }

        public string ValidationDrivingEvaluation(EvaluationInputViewModel evaluation)
        {
            var drivingEval = _evaluationService.GetEvaluationDrivingById(ApiConstants.UserId, evaluation.IdDriving).Result;
            if (drivingEval != null)
                return "Désole!!!!! Vous avez déjà fait une evaluation pour cette info de roulage.";

            if (evaluation.Value < 1 || evaluation.Value > 100)
                return "La valeur de l'évaluation est un nombre entre 1 et 100 ";

            return ApiConstants.OkMessage;
        }

        public bool CheckEmailAddressFormat(string emailaddress)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(emailaddress);
                var ret = match.Success;
                return ret;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool CheckIfEmailAddressNotAlreadyExisting(string emailaddress)
        {
            UserEmailModel emailInput = new UserEmailModel() { EmailAddress = emailaddress };
            var user = _userService.GetUserByEmail(emailInput).Result;
            if (user == null)
                return false;

            return true;
        }

        public bool CheckValidPassoword(string password)
        {
            try
            {
                Regex rx1 = new Regex("[a-z]+");
                Match match1 = rx1.Match(password);
                if (!match1.Success)
                    return false;

                Regex rx2 = new Regex("[A-Z]+");
                Match match2 = rx2.Match(password);
                if (!match2.Success)
                    return false;

                Regex rx3 = new Regex("[0-9]+");
                Match match3 = rx3.Match(password);
                if (!match3.Success)
                    return false;

                Regex rx4 = new Regex("[&~#^{_@}|]+");
                Match match4 = rx4.Match(password);
                if (!match4.Success)
                    return false;

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool CheckIfExistingUsername(string username)
        {
            UsernameModel usernameInput = new UsernameModel() { UserName = username };
            var user = _userService.GetUserByUsername(usernameInput);
            if (user == null)
                return false;

            return true;
        }

        public bool CheckIfParkingExistingAlredy(decimal latitude, decimal longitude)
        {
            GeographicPointModel point = new GeographicPointModel() { Latitude = latitude, Longitude = longitude };
            var park = _parkingInfoService.GetParkingByPointGeographic(point).Result;
            if (park == null)
                return false;

            return true;
        }
    }
}
