using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;

namespace NamRider.Util
{ 
    //these classes are used to define all constants linked to classes of the app
    
    public class AppConstants
    {
        public const string MAP_KEY = "rrF8EUqVgiU45CJtpUKe~34vmHIgnNXrb6csANb-9Qw~AjATsywbu_HPIevbhHZKieHvaWJigMo6NYoM2mdLCb9XzmDnyrYc0DlLGLO3XQVY";
        public static readonly Geopoint NAMUR_GEOPOINT = new Geopoint(new BasicGeoposition() { Latitude = 50.4669, Longitude = 4.86746 });
        public const string Collapsed = "Collapsed";
        public const string Visible = "Visible";
    }
    public static class ApiConstants
    {
        public static string Token { get; set; }
        public static string Username { get; set; }
        public static string UserId { get; set; }

        public const string OkMessage = "Ok";
        public const string AuthorizationTitle = "Authorization";
        public const string FormatJson = "application/json";
        public const string FormatToken = "application/x-www-form-urlencoded";
        public const string UsernameToken = "Username=";
        public const string PasswordToken = "&Password=";
        public const string GrantTypeToken = "&grant_type=password";
        public const string TokenApi = "http://namriderapi.azurewebsites.net/token";
        internal const string Api = "http://namriderapi.azurewebsites.net/api/";

        public class UserApiMethod
        {
            private const string TestApi = Api + "Account";
            private const string UserApi = Api + "User";
            public const string Register = TestApi + "/Register";
            public const string GetById = UserApi + "/{0}";
            public const string GetByUsername = UserApi + "/GetByUserName";
            public const string GetByEmail = UserApi + "/GetByEmail";
        }

        public class DrivingInfoApiMethod
        {
            private const string TestApi = Api + "DrivingInfo";
            public const string GetAllDrivingInfo = TestApi;
            public const string GetDrivingById = TestApi + "/{0}";
            public const string AddDriving = TestApi;
            public const string DeleteDriving = TestApi + "/{0}";
            public const string ReportedDated = TestApi + "/{0}";
        }

        public class ParkingInfoApiMethod
        {
            private const string TestApi = Api + "ParkingInfo";
            public const string GetAllParkingInfo = TestApi;
            public const string GetParkingById = TestApi + "/{0}";
            public const string AddParking = TestApi;
            public const string GetParkingByType = TestApi + "/GetByType";
            public const string DeleteParking = TestApi + "/{0}";
            public const string GetParkingByPointGeographic = TestApi + "/GetParkingByPoint";
            public const string ReportedDated = TestApi + "/{0}";
        }


        public class CriticismDrivingApiMethod
        {
            private const string TestApi = Api + "CriticismDriving";
            public const string GetAllCriticismDriving = TestApi;
            public const string GetCriticismDrivingById = TestApi + "/GetByIdDrivingCritism/{0}/{1}";
            public const string AddCriticismDriving = TestApi;
            public const string DeleteCriticismDriving = TestApi + "/DeleteDrivingCritism/{0}/{1}";
        }

        public class CriticismParkingApiMethod
        {
            private const string TestApi = Api + "CriticismParking";
            public const string GetAllCriticismParking = TestApi;
            public const string GetCriticismParkingById = TestApi + "/GetByIdParkingCritism/{0}/{1}";
            public const string AddCriticismParking = TestApi;
            public const string DeleteCriticismParking = TestApi + "/DeleteParkingCritism/{0}/{1}";
        }

        public class EvaluationApiMethod
        {
            private const string TestApi = Api + "Evaluation";
            public const string GetAllEvaluation = TestApi;
            public const string GetEvaluationById = TestApi + "/GetByIdEvaluation/{0}/{1}";
            public const string AddEvaluation = TestApi;
            public const string DeleteEvaluation = TestApi + "/DeleteEvaluation/{0}/{1}";
        }

    }

}
