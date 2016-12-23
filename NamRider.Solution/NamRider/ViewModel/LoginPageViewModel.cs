using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using NamRider.Model;
using NamRider.DAO;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using NamRider.Util;
using NamRider.Validations;

namespace NamRider.ViewModel
{
    public class LoginPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private UserService _service = new UserService();
        private ValidationClass _userValidation = new ValidationClass();
        private INavigationService _navigationService;
        public LoginPageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
        }

        private ICommand _goToHomePageCommand;
        public ICommand GoToHomePageCommand
        {
            get
            {
                if (_goToHomePageCommand == null)
                    _goToHomePageCommand = new RelayCommand(() => GoToHomePage());
                return _goToHomePageCommand;
            }
        }


        private ICommand _goToHomePageLoggedCommand;
        public ICommand GoToHomePageLoggedCommand
        {
            get
            {
                if (_goToHomePageLoggedCommand == null)
                    _goToHomePageLoggedCommand = new RelayCommand(() => GoToHomePageLogged());
                return _goToHomePageLoggedCommand;
            }
        }
        private void GoToHomePageLogged()
        {
            var login = LoginUser();
            if (login.IsSuccess)
            {
                GoToHomePage();
            }
            else
            {
                var mes = new MessageDialog(login.ErrorMsg);
                mes.ShowAsync();
            }
        }

        private void GoToHomePage()
        {
            _navigationService.NavigateTo("HomePage");
        }

        private string _userName;
        public string MyUserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                if (_userName != null)
                {
                    RaisePropertyChanged("MyUserName");
                }
            }
        }
        private ICommand _goToRegisterPageCommand;
        public ICommand GoToRegisterPageCommand
        {
            get
            {
                if (_goToRegisterPageCommand == null)
                    _goToRegisterPageCommand = new RelayCommand(() => GoToRegisterPage());
                return _goToRegisterPageCommand;
            }
        }
        private void GoToRegisterPage()
        {
            _navigationService.NavigateTo("RegisterPage");
        }

        private string _userPassword;
        public string MyUserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                if (_userPassword != null)
                {
                    RaisePropertyChanged("MyUserPassword");
                }
            }
        }


        public Response LoginUser()
        {
            UserLogin userLogin = new UserLogin() { UserName = this.MyUserName, Password = MyUserPassword };
            string valiMsg = _userValidation.UserLoginDataValidation(userLogin);
            if (valiMsg.Equals(ApiConstants.OkMessage))
            {
                var logingResult = _service.LoginUser(userLogin).Result;
                return logingResult;
            }
            else
            {
                Response response = new Response();
                response.IsSuccess = false;
                response.ErrorMsg = valiMsg;
                return response;
            }
        }
    }
}
