using DAT154Oblig4DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DAT154Oblig4DesktopApp
{
    /// <summary>
    /// Interaction logic for RoomPage.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        Dat154oblig4Context dataEntities = new Dat154oblig4Context();
        public RoomPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Getting data for reservations

            var query =
                from rooms in dataEntities.Rooms
                orderby rooms.Number
                select new { rooms.Id, rooms.Number, rooms.Beds, rooms.Quality, rooms.Size };

            dataGrid1.ItemsSource = query.ToList();

        }

        private void OnGoToReservationPageClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
        }

        private void OnGoToNewTaskPageClicked(object sender, RoutedEventArgs e)
        {
            var myValue = (int)((Button)sender).Tag;

            NavigationService.Navigate(new NewRoomTaskPage(myValue));
        }
    }
}
