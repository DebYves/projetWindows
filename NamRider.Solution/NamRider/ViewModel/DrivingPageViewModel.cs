using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using NamRider.DAO;
using NamRider.Model;
using NamRider.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace NamRider.ViewModel
{
    public class DrivingPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;

        [PreferredConstructor]//useless car il n'y a qu'un constructeur
        public DrivingPageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
            InitializeAsync();
            ShowDrivingInfo = new RelayCommand<DrivingInfoModel>(ShowDrivingInfoAction);
        }
        public void OnNavigatedTo(NavigationEventArgs e)
        {

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

        //Gestion de la map dans le ViewModel
        private Geopoint _startLocation = AppConstants.NAMUR_GEOPOINT;
        public Geopoint StartLocation
        {
            get { return _startLocation; }
        }

        private ObservableCollection<DrivingInfoModel> _drivingInfos = null;
        public ObservableCollection<DrivingInfoModel> DrivingInfos
        {
            get { return _drivingInfos; }
            set
            {
                if (_drivingInfos == value)
                {
                    return;
                }
                _drivingInfos = value;
                RaisePropertyChanged("DrivingInfos");
            }
        }
        
        public async Task InitializeAsync()
        {
            var service = new DrivingInfoService();
            var drivingInfos = await service.GetAllDrivingInfo();
            DrivingInfos = new ObservableCollection<DrivingInfoModel>(drivingInfos);
        }


        private RelayCommand<DrivingInfoModel> _showDrivingInfo = null;
        public RelayCommand<DrivingInfoModel> ShowDrivingInfo
        {
            get { return _showDrivingInfo; }
            set
            {
                if (_showDrivingInfo == value)
                {
                    return;
                }

                _showDrivingInfo = value;
                RaisePropertyChanged("ShowDrivingInfo");
            }
        }
        
        //Variable à cette portée de manière a pouvoir y avoir accès dans la méthode qui permet de fermer le popup via le bouton
        private ContentDialog popupInfo;
        //idem pour critiquer l'info via le bouton
        private DrivingInfoModel _drivingInfoPopup;

        public DrivingInfoModel SelectedDrivingInfo
        {
            get { return _drivingInfoPopup; }
            set
            {
                _drivingInfoPopup = value;
                if (_drivingInfoPopup != null)
                {
                    RaisePropertyChanged("SelectedDrivingInfo");
                }
            }
        }
        private void ShowDrivingInfoAction(DrivingInfoModel drivingInfo)
        {
            SelectedDrivingInfo = drivingInfo;
            popupInfo = new ContentDialog()
            {
                Title = "Description du danger"
            };
            var panel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };

            var dateInfo = new TextBlock()
            {
                Text = "Date de publication : " + drivingInfo.Date.ToString(),
                TextWrapping = TextWrapping.Wrap
            };
            var pertinenceInfo = new TextBlock()
            {
                Text = "Niveau de pertinence : " + drivingInfo.ValuePertinence.ToString(),
                TextWrapping = TextWrapping.Wrap
            };
            var streetInfo = new TextBlock()
            {
                Text = "Rue : " + drivingInfo.StreetName,
                TextWrapping = TextWrapping.Wrap
            };
            var severityInfo = new TextBlock()
            {
                Text = "Niveau de gravité : " + drivingInfo.Severity.ToString(),
                TextWrapping = TextWrapping.Wrap
            };
            var outdatedInfo = new TextBlock()
            {
                Text = "Signalée dépassée : " + drivingInfo.IsReportedOutDated.ToString()
            };

            var description = new TextBlock()
            {
                Text = "Description : ",
                TextWrapping = TextWrapping.Wrap
            };
            var descriptionInfoBox = new Border()
            {
                BorderThickness = new Thickness(1.0),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
            var descriptionInfo = new TextBlock()
            {
                Text = drivingInfo.Description,
                TextWrapping = TextWrapping.Wrap
            };

            panel.Children.Add(dateInfo);
            panel.Children.Add(pertinenceInfo);
            panel.Children.Add(streetInfo);
            panel.Children.Add(severityInfo);
            panel.Children.Add(description);
            descriptionInfoBox.Child = descriptionInfo;
            panel.Children.Add(descriptionInfoBox);

            //s'affiche si y en a une
            if (Utils.StringDefined(drivingInfo.AdditionalInfo))
            {
                var additionalInfo = new TextBlock()
                {
                    Text = "Informations supplémentaires : ",
                    TextWrapping = TextWrapping.Wrap
                };
                var infoSupInfoBox = new Border()
                {
                    BorderThickness = new Thickness(1.0),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                var infoSupInfo = new TextBlock()
                {
                    Text = drivingInfo.AdditionalInfo,
                    TextWrapping = TextWrapping.Wrap
                };

                panel.Children.Add(additionalInfo);
                infoSupInfoBox.Child = infoSupInfo;
                panel.Children.Add(infoSupInfoBox);
            }
            var userNameInfo = new TextBlock()
            {
                Text = drivingInfo.UserName,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            panel.Children.Add(userNameInfo);

            //Panneau des boutons
            var buttonPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //Bouton pour fermer
            var fermer = new Button() {
                Margin = new Thickness(5),
                Content = "Fermer"
            };
            //Bouton pour critiquer
            var criticize = new Button()
            {
                Margin = new Thickness(5),
                Content = "Critiquer"
            };
            fermer.Click += close_click;
            criticize.Click += criticize_click;
            buttonPanel.Children.Add(fermer);
            buttonPanel.Children.Add(criticize);
            panel.Children.Add(buttonPanel);
            popupInfo.Content = panel;
            popupInfo.ShowAsync();
        }
        private void close_click(object sender, RoutedEventArgs e)
        {
            popupInfo.Hide();
        }
        private void criticize_click(object sender, RoutedEventArgs e)
        {
            popupInfo.Hide();
            GoToDrivingInfoPage();
        }

        private ICommand _goToDrivingInfoPageCommand;
        public ICommand GoToDrivingInfoPageCommand
        {
            get
            {
                if (_goToDrivingInfoPageCommand == null)
                    _goToDrivingInfoPageCommand = new RelayCommand(() => GoToDrivingInfoPage());
                return _goToDrivingInfoPageCommand;
            }
        }
        private void GoToDrivingInfoPage()
        {
            _navigationService.NavigateTo("DrivingInfoPage", SelectedDrivingInfo);
        }

        //Partie reçue depuis le code-behind de la page correspondante
        private DrivingInfoModel _addedDrivingInfo;
        public DrivingInfoModel AddedDrivingInfo
        {
            get { return _addedDrivingInfo; }
            set
            {
                _addedDrivingInfo = value;
                if (_addedDrivingInfo != null)
                {
                    RaisePropertyChanged("AddedDrivingInfo");
                }
            }
        }

        private AddInfoPopup addInfoPopup;
        public void NewInfoBinding(Geopoint receivedGeoPointFromCodeBehind, string receivedStreetFromCodeBehind)
        {
            if (AddIndicationVisibility == AppConstants.Visible)
            {
                AddedDrivingInfo = new DrivingInfoModel();
                AddedDrivingInfo.Latitude = receivedGeoPointFromCodeBehind.Position.Latitude;
                AddedDrivingInfo.Longitude = receivedGeoPointFromCodeBehind.Position.Longitude;
                AddedDrivingInfo.StreetName = receivedStreetFromCodeBehind;

                addInfoPopup = new AddInfoPopup("Ajout d'un danger", receivedStreetFromCodeBehind);
                //Binder les boutons du popup
                addInfoPopup.CloseButton.Click += addClose_click;
                addInfoPopup.ForwardButton.Click += addForward_click;

                addInfoPopup.Popup.ShowAsync();

                AddButtonVisibility = Utils.VisibilityInverter(AddButtonVisibility);
                AddIndicationVisibility = Utils.VisibilityInverter(AddIndicationVisibility);
            }
        }

        //gestion des boutons du AddInfoPopup
        private void addClose_click(object sender, RoutedEventArgs e)
        {
            addInfoPopup.Popup.Hide();
        }
        private void addForward_click(object sender, RoutedEventArgs e)
        {
            addInfoPopup.Popup.Hide();
            GoToAddDrivingInfoPage();
        }

        //Transforme le bouton d'ajout en un texte
        private string _addButtonVisibility = AppConstants.Visible;
        public string AddButtonVisibility
        {
            get { return _addButtonVisibility; }
            set
            {
                _addButtonVisibility = value;
                RaisePropertyChanged("AddButtonVisibility");
            }
        }

        private string _addIndicationVisibility = AppConstants.Collapsed;
        public string AddIndicationVisibility
        {
            get { return _addIndicationVisibility; }
            set
            {
                _addIndicationVisibility = value;
                RaisePropertyChanged("AddIndicationVisibility");
            }
        }
        private ICommand _buttonVisibility = null;
        public ICommand ButtonVisibility
        {
            get
            {
                if (_buttonVisibility == null)
                    _buttonVisibility = new RelayCommand(() => ButtonVisibilityAction());
                return _buttonVisibility;
            }
        }
        private void ButtonVisibilityAction()
        {
            if (AddIndicationVisibility == AppConstants.Collapsed)
            {
                //L'un disparait l'autre apparait
                AddButtonVisibility = Utils.VisibilityInverter(AddButtonVisibility);
                AddIndicationVisibility = Utils.VisibilityInverter(AddIndicationVisibility);
            }
        }

        private ICommand _goToAddDrivingInfoPageCommand;
        public ICommand GoToAddDrivingInfoPageCommand
        {
            get
            {
                if (_goToAddDrivingInfoPageCommand == null)
                    _goToAddDrivingInfoPageCommand = new RelayCommand(() => GoToAddDrivingInfoPage());
                return _goToAddDrivingInfoPageCommand;
            }
        }
        private void GoToAddDrivingInfoPage()
        {
            _navigationService.NavigateTo("AddDrivingInfoPage", AddedDrivingInfo);
        }

    }
}