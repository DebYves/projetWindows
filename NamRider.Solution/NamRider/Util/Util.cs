using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NamRider.Util
{
    public class Utils
    {
        public static bool StringDefined(string s)
        {
            return (s != null && s != "");
        }
        public static string VisibilityInverter(string s)
        {
            if (s == AppConstants.Collapsed)
            {
                return AppConstants.Visible;
            }
            return AppConstants.Collapsed;
        }
        //Méthode pour obtenir des points permettant de dessiner un cercle sur la map
        //Source : https://dzone.com/articles/drawing-circles-windows-phone
        public static BasicGeoposition GetAtDistanceBearing(Geopoint point, double distance, double bearing)
        {
            const double degreesToRadian = Math.PI / 180.0;
            const double radianToDegrees = 180.0 / Math.PI;
            const double earthRadius = 6378137.0;

            var latA = point.Position.Latitude * degreesToRadian;
            var lonA = point.Position.Longitude * degreesToRadian;
            var angularDistance = distance / earthRadius;
            var trueCourse = bearing * degreesToRadian;

            var lat = Math.Asin(
                Math.Sin(latA) * Math.Cos(angularDistance) +
                Math.Cos(latA) * Math.Sin(angularDistance) * Math.Cos(trueCourse));

            var dlon = Math.Atan2(
                Math.Sin(trueCourse) * Math.Sin(angularDistance) * Math.Cos(latA),
                Math.Cos(angularDistance) - Math.Sin(latA) * Math.Sin(lat));

            var lon = ((lonA + dlon + Math.PI) % (Math.PI * 2)) - Math.PI;

            return new BasicGeoposition() { Latitude = lat * radianToDegrees, Longitude = lon * radianToDegrees };
        }
        public static Geopath GetCirclePoints(Geopoint center, double radius)
        {
            int nrOfPoints = 20; //+ ce nombre est élevé et + le cercle est affiné 
            var angle = 360.0 / nrOfPoints;
            var locations = new List<BasicGeoposition>();
            for (var i = 0; i <= nrOfPoints; i++)
            {
                locations.Add(GetAtDistanceBearing(center, radius, angle * i));
            }
            return new Geopath(locations);
        }
    }

    //Popup d'ajout d'info. Il est le même que ce soit des drivingInfo ou parkingInfo
    //Popup pour l'entrée d'une valeur de pertinence ou gravité aussi (différent constructeur)
    //Popup pour signaler une information comme dépassée (constructeur sans argument)
    public class AddInfoPopup
    {
        public Button CloseButton { get; set; }
        public Button ForwardButton { get; set; }
        public ContentDialog Popup { get; set; }
        public TextBox InputText { get; set; }

        public AddInfoPopup(string title, string street)
        {
            Popup = new ContentDialog()
            {
                Title = title
            };
            var panel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };

            var pointedStreet = new TextBlock()
            {
                Text = "Rue concernée : " + street,
                TextWrapping = TextWrapping.Wrap
            };
            var prevention = new TextBlock()
            {
                Text = "Êtes-vous sûr(e) de vouloir ajouter une information à la carte?",
                TextWrapping = TextWrapping.Wrap
            };
            //Panneau des boutons
            var buttonPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //Bouton pour fermer
            CloseButton = new Button()
            {
                Margin = new Thickness(5),
                Content = "Annuler"
            };
            //Bouton pour continuer
            ForwardButton = new Button()
            {
                Margin = new Thickness(5),
                Content = "Continuer"
            };
            buttonPanel.Children.Add(ForwardButton);
            buttonPanel.Children.Add(CloseButton);

            panel.Children.Add(pointedStreet);
            panel.Children.Add(prevention);
            panel.Children.Add(buttonPanel);
            Popup.Content = panel;
        }

        public AddInfoPopup(string title)
        {
            Popup = new ContentDialog()
            {
                Title = title
            };
            var panel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };
            InputText = new TextBox();
            //Panneau des boutons
            var buttonPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //Bouton pour fermer
            CloseButton = new Button()
            {
                Margin = new Thickness(5),
                Content = "Annuler"
            };
            //Bouton pour continuer
            ForwardButton = new Button()
            {
                Margin = new Thickness(5),
                Content = "Valider"
            };
            buttonPanel.Children.Add(ForwardButton);
            buttonPanel.Children.Add(CloseButton);

            panel.Children.Add(InputText);
            panel.Children.Add(buttonPanel);
            Popup.Content = panel;
        }

        public int ValidateInputNumber()
        {
            int input;
            try {
                input = int.Parse(InputText.Text);
                if(input<0 || input > 100)
                {
                    input = -1;
                }
            }
            catch (FormatException e)
            {
                input = -1;
            }
            return input;
        }

        public AddInfoPopup()
        {
            Popup = new ContentDialog()
            {
                Title = "Voulez-vous signaler cette information comme n'étant plus valable?"
            };
            var panel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };
            //Panneau des boutons
            var buttonPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //Bouton pour fermer
            CloseButton = new Button()
            {
                Margin = new Thickness(5),
                Content = "Annuler"
            };
            //Bouton pour continuer
            ForwardButton = new Button()
            {
                Margin = new Thickness(5),
                Content = "Oui"
            };
            buttonPanel.Children.Add(ForwardButton);
            buttonPanel.Children.Add(CloseButton);

            panel.Children.Add(buttonPanel);
            Popup.Content = panel;
        }

    }
}
