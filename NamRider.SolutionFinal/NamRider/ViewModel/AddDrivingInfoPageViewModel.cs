using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using NamRider.DAO;
using NamRider.Model;
using NamRider.Util;
using NamRider.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace NamRider.ViewModel
{
    public class AddDrivingInfoPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private DrivingInfoService _service = new DrivingInfoService();
        private ValidationClass _userValidation = new ValidationClass();

        public DrivingInfoModel DrivingInfo { get; set; }
        private INavigationService _navigationService;

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                if (_description != null && _description.Length >= 2 || _description.Length <= 350)
                {
                    RaisePropertyChanged("Description");
                }
            }
        }


        private string _additionnalInfo;
        public string AdditionnalInfo
        {
            get { return _additionnalInfo; }
            set
            {
                _additionnalInfo = value;
                if (_additionnalInfo != null && _additionnalInfo.Length >= 2 && _description.Length <= 200)
                {
                    RaisePropertyChanged("AdditionnalInfo");
                }
            }
        }

        private int _severity;
        public int Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;
                if (_severity > 0 && _severity <= 100)
                {
                    RaisePropertyChanged("Severity");
                }
            }
        }

        public AddDrivingInfoPageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
        }
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            DrivingInfo = (DrivingInfoModel)e.Parameter;
        }
        private ICommand _goToDrivingPageCommand;
        public ICommand GoToDrivingPageCommand
        {
            get
            {
                if (_goToDrivingPageCommand == null)
                    _goToDrivingPageCommand = new RelayCommand(() => GoToDrivingPage());
                return _goToDrivingPageCommand;
            }
        }

        private ICommand _goAddDrivingPageCommand;
        public ICommand GoAddDrivingPageCommand
        {
            get
            {
                if (_goAddDrivingPageCommand == null)
                    _goAddDrivingPageCommand = new RelayCommand(() => GoToHomeDrivingPage());
                return _goAddDrivingPageCommand;
            }
        }

        private void GoToDrivingPage()
        {
            _navigationService.NavigateTo("DrivingPage");
        }

        private void GoToHomeDrivingPage()
        {
            var result = AddDriving();
            if (result.IsSuccess)
            {
                GoToDrivingPage();
            }
            else
            {
                var messageError = new MessageDialog(result.ErrorMsg);
                messageError.ShowAsync();
            }
        }
           

        public Response AddDriving()
        {
            Response response = new Response();
            response.IsSuccess = false;
            if (ApiConstants.Token != null)
            {
                decimal latitude = (decimal)DrivingInfo.Latitude; decimal longitude = (decimal)DrivingInfo.Longitude;
                var driving = new DrivingInfoInputModel() { Latitude = latitude, Longitude = longitude, StreetName = DrivingInfo.StreetName, Description = Description, AdditionalInfo = AdditionnalInfo };
                string testData = _userValidation.DrivingValidation(driving);
                if (testData.Equals(ApiConstants.OkMessage))
                {
                    var drivingAddResult = _service.AddDriving(driving).Result;
                    if (drivingAddResult.IsSuccess)
                    {
                        var evaluationDriving = new EvaluationInputViewModel() { IdDriving = drivingAddResult.Id, Value = Severity };
                        var _evaluation = new EvaluationService();
                        var resultAdd = _evaluation.AddEvaluationDriving(evaluationDriving).Result;

                        var critismDriving = new CriticismDrivingInputModel() { IdDriving = drivingAddResult.Id, Value = 100 };
                        var _critism = new CriticismService();
                        var resultAddC = _critism.AddCritismDriving(critismDriving).Result;

                        return drivingAddResult;
                    }
                    else
                        response.ErrorMsg = "Ajout de l'info de roulage impossible";
                }
                else
                    response.ErrorMsg = testData;
                return response;
            }
            response.ErrorMsg = "Vous devez être connecté(e) pour cette action"; ;
            return response;
        }

    }
}
