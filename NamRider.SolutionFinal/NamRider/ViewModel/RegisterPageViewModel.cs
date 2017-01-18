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

namespace NamRider.ViewModel
{
    public class RegisterPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private UserService _service = new UserService();
        private ValidationClass _userValidation = new ValidationClass();

        private INavigationService _navigationService;
        public RegisterPageViewModel(INavigationService navigationService = null)
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
        private void GoToHomePage()
        {
            _navigationService.NavigateTo("HomePage");
        }


        private ICommand _goToHomePageRegisterCommand;
        public ICommand GoToHomePageRegisterCommand
        {
            get
            {
                if (_goToHomePageRegisterCommand == null)
                    _goToHomePageRegisterCommand = new RelayCommand(() => GoToHomePageRegister());
                return _goToHomePageRegisterCommand;
            }
        }
        private void GoToHomePageRegister()
        {
            var user = RegisterUser();
            if (user.IsSuccess)
            {
                GoToHomePage();
            }
            else
            {
                var mes = new MessageDialog(user.ErrorMsg);
                mes.ShowAsync();
            }
        }

        private string _email;
        public string MyEmail
        {
            get { return _email; }
            set
            {
                _email = value;
                if (_email != null)
                {
                    RaisePropertyChanged("MyEmail");
                }
            }
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

        private string _userPasswordConfirm;
        public string MyUserPasswordConfirm
        {
            get { return _userPasswordConfirm; }
            set
            {
                _userPasswordConfirm = value;
                if (_userPasswordConfirm != null)
                {
                    RaisePropertyChanged("MyUserPasswordConfirm");
                }
            }
        }

        public Response RegisterUser()
        {
            UserInput userRegister = new UserInput() { Email = MyEmail, UserName = MyUserName, Password = MyUserPassword, ConfirmPassword = MyUserPasswordConfirm };
            string testData = _userValidation.UserRegisterDataValidation(userRegister);
            if (testData.Equals(ApiConstants.OkMessage))
            {
                var registerResult = _service.RegisterUser(userRegister).Result;
                return registerResult;
            }
            else
            {
                Response response = new Response();
                response.IsSuccess = false;
                response.ErrorMsg = testData;
                return response;
            }
        }
    }
}
