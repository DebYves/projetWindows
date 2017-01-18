using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using NamRider.DAO;
using NamRider.Model;
using NamRider.Util;
using NamRider.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace NamRider.ViewModel
{
    public class AddParkingInfoPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ParkingInfoService _service = new ParkingInfoService();
        private ValidationClass _userValidation = new ValidationClass();

        public ParkingInfoModel ParkingInfo { get; set; }
        private INavigationService _navigationService;

        public ObservableCollection<string> TypeList
        {
            get { return new ObservableCollection<string> { "Disque", "Gratuit", "Alternance", "Payant", "Autre" }; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                if (_description != null && _description.Length <= 350)
                {
                    RaisePropertyChanged("Description");
                }
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (_type != null)
                {
                    RaisePropertyChanged("Type");
                }
            }
        }

        private int _radius;
        public int Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                if (_radius > 0 && _radius <= 999)
                {
                    RaisePropertyChanged("Radius");
                }
            }
        }

        private int _numberPlace;
        public int NumberPlace
        {
            get { return _numberPlace; }
            set
            {
                _numberPlace = value;
                _numberPlace = (_numberPlace > 0) ? _numberPlace : 1;
                if (_numberPlace > 0 && _numberPlace <= 999)
                {
                    RaisePropertyChanged("NumberPlace");
                }
            }
        }

        private bool _isEstimated;
        public bool IsEstimated
        {
            get { return _isEstimated; }
            set
            {
                _isEstimated = value;
                RaisePropertyChanged("IsEstimated");
            }
        }

        public AddParkingInfoPageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
        }
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            ParkingInfo = (ParkingInfoModel)e.Parameter;
        }

        private ICommand _goToParkingPageCommand;
        public ICommand GoToParkingPageCommand
        {
            get
            {
                if (_goToParkingPageCommand == null)
                    _goToParkingPageCommand = new RelayCommand(() => GoToParkingPage());
                return _goToParkingPageCommand;
            }
        }
        private void GoToParkingPage()
        {
            _navigationService.NavigateTo("ParkingPage");
        }

        private ICommand _goAddParkingPageCommand;
        public ICommand GoAddParkingPageCommand
        {
            get
            {
                if (_goAddParkingPageCommand == null)
                    _goAddParkingPageCommand = new RelayCommand(() => GoToHomeParkingPage());
                return _goAddParkingPageCommand;
            }
        }

        private void GoToHomeParkingPage()
        {
            var result = AddParking();
            if (result.IsSuccess)
            {
                GoToParkingPage();
            }
            else
            {
                var messageError = new MessageDialog(result.ErrorMsg);
                messageError.ShowAsync();
            }
        }

        public Response AddParking()
        {
            Response response = new Response();
            response.IsSuccess = false;
            if (ApiConstants.Token != null)
            {
                decimal latitude = (decimal)ParkingInfo.Latitude; decimal longitude = (decimal)ParkingInfo.Longitude;
                var park = new ParkingInfoInputModel() { Rayon = Radius, Type = Type, Latitude = latitude, Longitude = longitude, Description = Description, IsEstimatedPlace = IsEstimated };
                park.NumberPlace = (NumberPlace > 0) ? NumberPlace : 1;
                string testData = _userValidation.ParkingValidation(park);
                if (testData.Equals(ApiConstants.OkMessage))
                {
                    var parkAddResult = _service.AddParking(park).Result;
                    if (parkAddResult.IsSuccess)
                    {
                        var critismPark = new CriticismParkingInputModel() { IdParking = parkAddResult.Id, Value = 100 };
                        var _critism = new CriticismService();
                        var resultAddC = _critism.AddCritismParking(critismPark).Result;

                        return parkAddResult;
                    }
                    else
                        response.ErrorMsg = "Ajout du parking impossible";
                }
                else
                    response.ErrorMsg = testData;
                return response;
            }
            response.ErrorMsg = "Vous devez être connecté(e) pour cette action";
            return response;
        }

    }
}
