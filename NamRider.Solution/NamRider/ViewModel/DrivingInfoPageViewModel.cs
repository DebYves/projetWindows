using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NamRider.ViewModel
{
    public class DrivingInfoPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public DrivingInfoModel SelectedDrivingInfo{ get; set; }
        private ValidationClass _userValidation = new ValidationClass();

        private INavigationService _navigationService;
        [PreferredConstructor]
        public DrivingInfoPageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
        }


        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedDrivingInfo = (DrivingInfoModel)e.Parameter;
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
        private void GoToDrivingPage()
        {
            _navigationService.NavigateTo("DrivingPage");
        }
        public string AdditionalInfoVisibility
        {
            get
            {
                if (!Utils.StringDefined(SelectedDrivingInfo.AdditionalInfo))
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
            get {
                if (_showCriticism == null)
                    _showCriticism = new RelayCommand(() => ShowCriticismAction());
                    return _showCriticism; }
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
            if (ApiConstants.Token == null)
            {
                int input = criticismPopup.ValidateInputNumber();
                var criticismDriving = new CriticismDrivingInputModel() { IdDriving = SelectedDrivingInfo.Id, Value = input };
                string validCritism = _userValidation.ValidationDrivingCritism(criticismDriving);

                if (validCritism.Equals(ApiConstants.OkMessage))
                {
                    var _criticism = new CriticismService();
                    var resultAdd = _criticism.AddCritismDriving(criticismDriving).Result;
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

        //Critique de gravité
        private ICommand _showSeverityCriticism = null;
        public ICommand ShowSeverityCriticism
        {
            get
            {
                if (_showSeverityCriticism == null)
                    _showSeverityCriticism = new RelayCommand(() => ShowSeverityCriticismAction());
                return _showSeverityCriticism;
            }
            set
            {
                if (_showSeverityCriticism == value)
                {
                    return;
                }
                _showSeverityCriticism = value;
                RaisePropertyChanged("ShowSeverityCriticism");
            }
        }
        private AddInfoPopup severityCriticismPopup;//ContentDialog popupCriticism;
        private void ShowSeverityCriticismAction()
        {
            severityCriticismPopup = new AddInfoPopup("Donner une valeur de gravité entre 0 et 100");
            //Binder les boutons du popup
            severityCriticismPopup.CloseButton.Click += closeSeverityCriticism_click;
            severityCriticismPopup.ForwardButton.Click += validateSeverityCriticism_click;
            severityCriticismPopup.Popup.ShowAsync();
        }
        private void closeSeverityCriticism_click(object sender, RoutedEventArgs e)
        {
            severityCriticismPopup.Popup.Hide();
        }

        private void validateSeverityCriticism_click(object sender, RoutedEventArgs e)
        {
            if (ApiConstants.Token == null)
            {
                int input = severityCriticismPopup.ValidateInputNumber();
                var evaluation = new EvaluationInputViewModel() { IdDriving = SelectedDrivingInfo.Id, Value = input };
                string validvaluation = _userValidation.ValidationDrivingEvaluation(evaluation);
                if (validvaluation.Equals(ApiConstants.OkMessage))
                {
                    var _evaluation = new EvaluationService();
                    var resulAddEvaluation = _evaluation.AddEvaluationDriving(evaluation).Result;
                    if (!resulAddEvaluation.IsSuccess)
                    {
                        var messageError = new MessageDialog("Impossible d'ajouter l'évaluation");
                        messageError.ShowAsync();
                    }
                    else
                        severityCriticismPopup.Popup.Hide();
                }
                else
                {
                    var messageError = new MessageDialog(validvaluation);
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
            if (ApiConstants.Token == null)
            {
                var reporteddate = new DrivingInfoEditModel() { IsReported = true };
                var _service = new DrivingInfoService();
                var resultAlert = _service.ReportedOutDated(SelectedDrivingInfo.Id, reporteddate).Result;
                if (!resultAlert.IsSuccess)
                {
                    var messageError = new MessageDialog(resultAlert.ErrorMsg);
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
