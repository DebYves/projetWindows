using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using NamRider.View;

namespace NamRider.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //MVVM
            SimpleIoc.Default.Register<LoginPageViewModel>();
            SimpleIoc.Default.Register<RegisterPageViewModel>();

            SimpleIoc.Default.Register<HomePageViewModel>();

            SimpleIoc.Default.Register<DrivingPageViewModel>();
            SimpleIoc.Default.Register<DrivingInfoPageViewModel>();
            SimpleIoc.Default.Register<AddDrivingInfoPageViewModel>();

            SimpleIoc.Default.Register<ParkingPageViewModel>();
            SimpleIoc.Default.Register<ParkingInfoPageViewModel>();
            SimpleIoc.Default.Register<AddParkingInfoPageViewModel>();

            NavigationService navigationPages = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationPages);
            //MVVM
            navigationPages.Configure("LoginPage", typeof(LoginPage));
            navigationPages.Configure("RegisterPage", typeof(RegisterPage));

            navigationPages.Configure("HomePage", typeof(HomePage));

            navigationPages.Configure("DrivingPage", typeof(DrivingPage));
            navigationPages.Configure("DrivingInfoPage", typeof(DrivingInfoPage));
            navigationPages.Configure("AddDrivingInfoPage", typeof(AddDrivingInfoPage));

            navigationPages.Configure("ParkingPage", typeof(ParkingPage));
            navigationPages.Configure("ParkingInfoPage", typeof(ParkingInfoPage));
            navigationPages.Configure("AddParkingInfoPage", typeof(AddParkingInfoPage));

        }

        //MVVM
        public LoginPageViewModel Login
        {
            get { return ServiceLocator.Current.GetInstance<LoginPageViewModel>(); }
        }
        public RegisterPageViewModel Register
        {
            get { return ServiceLocator.Current.GetInstance<RegisterPageViewModel>(); }
        }

        public HomePageViewModel Home
        {
            get { return ServiceLocator.Current.GetInstance<HomePageViewModel>(); }
        }

        public DrivingPageViewModel Driving
        {
            get { return ServiceLocator.Current.GetInstance<DrivingPageViewModel>(); }
        }
        public DrivingInfoPageViewModel DrivingInfo
        {
            get { return ServiceLocator.Current.GetInstance<DrivingInfoPageViewModel>(); }
        }
        public AddDrivingInfoPageViewModel AddDrivingInfo
        {
            get { return ServiceLocator.Current.GetInstance<AddDrivingInfoPageViewModel>(); }
        }

        public ParkingPageViewModel Parking
        {
            get { return ServiceLocator.Current.GetInstance<ParkingPageViewModel>(); }
        }
        public ParkingInfoPageViewModel ParkingInfo
        {
            get { return ServiceLocator.Current.GetInstance<ParkingInfoPageViewModel>(); }
        }
        public AddParkingInfoPageViewModel AddParkingInfo
        {
            get { return ServiceLocator.Current.GetInstance<AddParkingInfoPageViewModel>(); }
        }
    }
}
