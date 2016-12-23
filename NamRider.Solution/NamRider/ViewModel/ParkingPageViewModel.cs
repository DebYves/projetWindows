using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using NamRider.Model;
using NamRider.DAO;
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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace NamRider.ViewModel
{
    public class ParkingPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;

        public ParkingPageViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
            InitializeAsync();
            ShowParkingInfo = new RelayCommand<ParkingInfoModel>(ShowParkingInfoAction);
        }
        
        public RelayCommand<ParkingInfoModel> ShowParkingInfo { get; private set; }

        private async Task InitializeAsync()
        {
            var service = new ParkingInfoService();
            var parkingInfos = await service.GetAllParking();
            ParkingInfos = new ObservableCollection<ParkingInfoModel>(parkingInfos);
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
        private ObservableCollection<ParkingInfoModel> _parkingInfos = null;
        public ObservableCollection<ParkingInfoModel> ParkingInfos
        {
            get { return _parkingInfos; }
            set
            {
                if (_parkingInfos == value)
                {
                    return;
                }
                _parkingInfos = value;
                RaisePropertyChanged("ParkingInfos");
            }
        }

        private ContentDialog popupInfo;
        private ParkingInfoModel _parkingInfoPopup;
        public ParkingInfoModel SelectedParkingInfo
        {
            get { return _parkingInfoPopup; }
            set
            {
                _parkingInfoPopup = value;
                if (_parkingInfoPopup != null)
                {
                    RaisePropertyChanged("SelectedParkingInfo");
                }
            }
        }

        private void ShowParkingInfoAction(ParkingInfoModel parkingInfo)
        {
            SelectedParkingInfo = parkingInfo;
            popupInfo = new ContentDialog(){
                Title = "Description du parking"
            };
            var panel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };

            var typeInfo = new TextBlock()
            {
                Text = "Type : " + parkingInfo.Type,
                TextWrapping = TextWrapping.Wrap
            };
            var pertinenceInfo = new TextBlock()
            {
                Text = "Niveau de pertinence : " + parkingInfo.ValuePertinence.ToString(),
                TextWrapping = TextWrapping.Wrap
            };
            
            var nbPlaceInfo = new TextBlock()
            {
                
                Text = "Nombre de places : " + parkingInfo.NbPlaceAndEstimated,
                TextWrapping = TextWrapping.Wrap
            };

            var outdatedInfo = new TextBlock()
            {
                Text = "Signalée dépassée : " + parkingInfo.IsReportedOutDated.ToString()
            };

            
            panel.Children.Add(typeInfo);
            panel.Children.Add(pertinenceInfo);
            panel.Children.Add(nbPlaceInfo);
            

            //s'affiche si y en a une
            if (Utils.StringDefined(parkingInfo.Description))
            {
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
                    Text = parkingInfo.Description,
                    TextWrapping = TextWrapping.Wrap
                };

                panel.Children.Add(description);
                descriptionInfoBox.Child = descriptionInfo;
                panel.Children.Add(descriptionInfoBox);
            }
            var userNameInfo = new TextBlock()
            {
                Text = parkingInfo.UserName,
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
            var close = new Button()
            {
                Margin = new Thickness(5),
                Content = "Fermer"
            };
            //Bouton pour critiquer
            var criticize = new Button()
            {
                Margin = new Thickness(5),
                Content = "Critiquer"
            };
            close.Click += close_click;
            criticize.Click += criticize_click;
            buttonPanel.Children.Add(close);
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
            GoToParkingInfoPage();
        }

        private ICommand _goToParkingInfoPageCommand;
        public ICommand GoToParkingInfoPageCommand
        {
            get
            {
                if (_goToParkingInfoPageCommand == null)
                    _goToParkingInfoPageCommand = new RelayCommand(() => GoToParkingInfoPage());
                return _goToParkingInfoPageCommand;
            }
        }
        private void GoToParkingInfoPage()
        {
            _navigationService.NavigateTo("ParkingInfoPage", SelectedParkingInfo);
        }

        //Partie reçue depuis le code-behind de la page correspondante
        private ParkingInfoModel _addedParkingInfo;
        public ParkingInfoModel AddedParkingInfo
        {
            get { return _addedParkingInfo; }
            set
            {
                _addedParkingInfo = value;
                if (_addedParkingInfo != null)
                {
                    RaisePropertyChanged("AddedParkingInfo");
                }
            }
        }

        private AddInfoPopup addInfoPopup;
        public void NewInfoBinding(Geopoint receivedGeoPointFromCodeBehind, string receivedStreetFromCodeBehind)
        {
            if (AddIndicationVisibility == AppConstants.Visible)
            {
                AddedParkingInfo = new ParkingInfoModel();
                AddedParkingInfo.Latitude = receivedGeoPointFromCodeBehind.Position.Latitude;
                AddedParkingInfo.Longitude = receivedGeoPointFromCodeBehind.Position.Longitude;

                addInfoPopup = new AddInfoPopup("Ajout d'un parking", receivedStreetFromCodeBehind);
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
            GoToAddParkingInfoPage();
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

        private ICommand _goToAddParkingInfoPageCommand;
        public ICommand GoToAddParkingInfoPageCommand
        {
            get
            {
                if (_goToAddParkingInfoPageCommand == null)
                    _goToAddParkingInfoPageCommand = new RelayCommand(() => GoToAddParkingInfoPage());
                return _goToAddParkingInfoPageCommand;
            }
        }
        private void GoToAddParkingInfoPage()
        {
            _navigationService.NavigateTo("AddParkingInfoPage", AddedParkingInfo);
        }

        //Affichage des zones de parking sur la map (pas en MVVM)
        //Récupération de la map
        public async Task DrawParkingZone(MapControl parkingMap)
        {
            MapPolygon drawParking;
            //On bloque tant que c'est vide, cette méthode s'exécute dans un thread
            while (ParkingInfos == null)
            {
                await Task.Delay(2000); //check toutes les 2s si ParkingInfos est bind
            }

            foreach (ParkingInfoModel parkingInfo in ParkingInfos)
            {
                drawParking = new MapPolygon();
                drawParking.Path = Utils.GetCirclePoints(parkingInfo.Location, parkingInfo.Rayon);
                drawParking.ZIndex = 1;
                drawParking.StrokeColor = Colors.Black;
                drawParking.StrokeThickness = 3;
                drawParking.StrokeDashed = false;

                Color color;
                switch (parkingInfo.Type)
                {
                    case "Disque":
                        color = Colors.DarkBlue;
                        break;
                    case "Alternance":
                        color = Colors.Red;
                        break;
                    case "Payant":
                        color = Colors.DarkMagenta;
                        break;
                    case "Gratuit":
                        color = Colors.Green;
                        break;
                    default:
                        color = Colors.Yellow;
                        break;
                }
                color.A = 127;//Rend le cercle transparent
                drawParking.FillColor = color;
                parkingMap.MapElements.Add(drawParking);
            }
        }
    }
}
