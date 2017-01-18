using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace NamRider.ViewModel
{
    public class HomePageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;

        [PreferredConstructor]
        public HomePageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            
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

        private ICommand _goToLoginPageCommand;
        public ICommand GoToLoginPageCommand
        {
            get
            {
                if (_goToLoginPageCommand == null)
                    _goToLoginPageCommand = new RelayCommand(() => GoToLoginPage());
                return _goToLoginPageCommand;
            }
        }
        private void GoToLoginPage()
        {
            _navigationService.NavigateTo("LoginPage");
        }

    }
    
}
