using NamRider.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using NamRider.ViewModel;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using System.Text;
using Windows.UI.Popups;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NamRider.View
{
    public sealed partial class DrivingPage : Page
    {

        public DrivingPage()
        {
            this.InitializeComponent();
            mapInitializer();
        }

        private DrivingPageViewModel _myDataContext = null;
        private DrivingPageViewModel MyDataContext
        {
            get
            {
                if (_myDataContext == null)
                {
                    _myDataContext = (DrivingPageViewModel)DataContext;
                }
                return _myDataContext;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MyDataContext.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }


        private void mapInitializer()
        {
            mapControlDriving.MapServiceToken = AppConstants.MAP_KEY;
            //mapControlDriving.Center = AppConstants.NAMUR_GEOPOINT;
            zoomMapLevel();
        }

        //purement visuel, cela n'a aucune influence sur la logique de l'application
        private void zoomMapLevel()
        {
            mapControlDriving.ZoomLevel = 14;
        }
        private void TrafficFlowVisible_Checked(object sender, RoutedEventArgs e)
        {
            mapControlDriving.TrafficFlowVisible = true;
        }

        private void trafficFlowVisibleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mapControlDriving.TrafficFlowVisible = false;
        }
        private async void GetLocation(MapControl sender, MapInputEventArgs args)
        {
            Geopoint pointToReverseGeocode = new Geopoint(args.Location.Position);
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
            //afficher la pin sur la map
            string street = null;
            if (result.Status == MapLocationFinderStatus.Success)
            {
                //On récupère le nom de la rue
                street = result.Locations[0].Address.Street;
            }
            MyDataContext.NewInfoBinding(pointToReverseGeocode, street);
        }
    }
}
