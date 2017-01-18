using NamRider.Util;
using NamRider.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NamRider.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ParkingPage : Page
    {
        public ParkingPage()
        {
            this.InitializeComponent();
            mapInitializer();
        }

        //juste pour éviter de la duplication de code 
        private ParkingPageViewModel _myDataContext = null;
        private ParkingPageViewModel MyDataContext {
            get
            {
                if (_myDataContext == null)
                {
                    _myDataContext = (ParkingPageViewModel)DataContext;
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
            mapControlParking.MapServiceToken = AppConstants.MAP_KEY;
            //mapControlParking.Center = AppConstants.NAMUR_GEOPOINT;
            zoomMapLevel();
            //Affiche les cercles avec un thread
            DoCircles();
        }

        //purement visuel, cela n'a aucune influence sur la logique de l'application
        private void zoomMapLevel()
        {
            mapControlParking.ZoomLevel = 14;
        }

        //Pas faisable en MVVM en UWP pour le moment 
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

        //Méthode du thread qui dessine les cercles
        public async Task DoCircles()
        {
            MyDataContext.DrawParkingZone(mapControlParking);
        }
    }
}
