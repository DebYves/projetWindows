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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace NamRider.ViewModel
{
    public class ParkingInfoPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ValidationClass _userValidation = new ValidationClass();

        public ParkingInfoModel SelectedParkingInfo { get; set; }
        private INavigationService _navigationService;

        public ParkingInfoPageViewModel(INavigationService navigationService = null)
        {

            _navigationService = navigationService;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedParkingInfo = (ParkingInfoModel)e.Parameter;
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

        public string DescriptionVisibility {
            get
            {
                if (!Utils.StringDefined(SelectedParkingInfo.Description))
                {
                    return AppConstants.Collapsed;
                }
                return AppConstants.Visible;
            }
        }

        //Critique de pertinence
        private ICommand _showCriticism = null;
        public ICommand ShowCriticism
        {
            get
            {
                if (_showCriticism == null)
                    _showCriticism = new RelayCommand(() => ShowCriticismAction());
                return _showCriticism;
            }
            set
            {
                if (_showCriticism == value)
                {
                    return;
                }
                _showCriticism = value;
                RaisePropertyChanged("ShowCriticism");
            }
        }
        private AddInfoPopup criticismPopup;//ContentDialog popupCriticism;
        private void ShowCriticismAction()
        {
            criticismPopup = new AddInfoPopup("Donner une valeur de pertinence entre 0 et 100");
            //Binder les boutons du popup
            criticismPopup.CloseButton.Click += closeCriticism_click;
            criticismPopup.ForwardButton.Click += validateCriticism_click;
            criticismPopup.Popup.ShowAsync();
        }
        private void closeCriticism_click(object sender, RoutedEventArgs e)
        {
            criticismPopup.Popup.Hide();
        }
        private void validateCriticism_click(object sender, RoutedEventArgs e)
        {
            if (ApiConstants.Token != null)
            {
                int input = criticismPopup.ValidateInputNumber();
                var criticismPark = new CriticismParkingInputModel() { IdParking = SelectedParkingInfo.Id, Value = input };
                string validCritism = _userValidation.ValidationParkCritism(criticismPark);

                if (validCritism.Equals(ApiConstants.OkMessage))
                {
                    var _criticism = new CriticismService();
                    var resultAdd = _criticism.AddCritismParking(criticismPark).Result;
                    if (!resultAdd.IsSuccess)
                    {
                        var messageError = new MessageDialog("Impossible d'ajouter la critique");
                        messageError.ShowAsync();
                    }
                    else
                        criticismPopup.Popup.Hide();
                }
                else
                {
                    var messageError = new MessageDialog(validCritism);
                    messageError.ShowAsync();
                }
            }
            else
            {
                var messageError = new MessageDialog("Vous devez être connecté(e) pour cette action");
                messageError.ShowAsync();
            }
            
        }

        //Signaler l'information comme dépassée
        private ICommand _showReporting = null;
        public ICommand ShowReporting
        {
            get
            {
                if (_showReporting == null)
                    _showReporting = new RelayCommand(() => ShowReportingAction());
                return _showReporting;
            }
            set
            {
                if (_showReporting == value)
                {
                    return;
                }
                _showReporting = value;
                RaisePropertyChanged("ShowReporting");
            }
        }
        private AddInfoPopup reportingPopup;//ContentDialog popupCriticism;
        private void ShowReportingAction()
        {
            reportingPopup = new AddInfoPopup();
            //Binder les boutons du popup
            reportingPopup.CloseButton.Click += closeReporting_click;
            reportingPopup.ForwardButton.Click += validateReporting_click;
            reportingPopup.Popup.ShowAsync();
        }
        private void closeReporting_click(object sender, RoutedEventArgs e)
        {
            reportingPopup.Popup.Hide();
        }
        private void validateReporting_click(object sender, RoutedEventArgs e)
        {
            if (ApiConstants.Token != null)
            {
                var reportedDate = new ParkingInfoEditModel() { IsReportedDate = true };
                var _service = new ParkingInfoService();
                var resulAlert = _service.ReportedOutDated(SelectedParkingInfo.Id, reportedDate).Result;
                if (!resulAlert.IsSuccess)
                {
                    var messageError = new MessageDialog("");
                    messageError.ShowAsync();
                }
                else
                    reportingPopup.Popup.Hide();
            }
            else
            {
                var messageError = new MessageDialog("Vous devez être connecté(e) pour cette action");
                messageError.ShowAsync();
            }
           
        }

    }
}
