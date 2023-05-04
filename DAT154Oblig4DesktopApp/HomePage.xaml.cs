using DAT154Oblig4DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        Dat154oblig4Context dataEntities = new Dat154oblig4Context();
        public HomePage()
        {
            InitializeComponent();

            CommandBinding binding = new CommandBinding(DataGrid.DeleteCommand, OnDeleteCommandExecuted);
            dataGrid1.CommandBindings.Add(binding);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Getting data for reservations

            var query =
                from reservations in dataEntities.Reservations
                orderby reservations.StartDate
                select new { reservations.Id, reservations.StartDate, reservations.EndDate, reservations.Room.Number, reservations.Customer.Name };

            dataGrid1.ItemsSource = query.ToList();

        }

        private void OnDeleteCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is int reservationId)
            {
                var reservationToDelete = dataEntities.Reservations.Find(reservationId);

                if (reservationToDelete != null)
                {
                    dataEntities.Reservations.Remove(reservationToDelete);
                    dataEntities.SaveChanges();

                    MessageBox.Show("Row deleted.");
                    NavigationService.Refresh();

                    return;
                }
            }

            MessageBox.Show("Unable to delete row.");
            NavigationService.Refresh();
        }

        private void OnGoToNewReservationPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("NewReservation.xaml", UriKind.Relative));
        }

        private void OnGoToRoomPageClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("RoomPage.xaml", UriKind.Relative));
        }

        private void OnGoToEditReservationPage(object sender, RoutedEventArgs e)
        {
            var myValue = (int)((Button)sender).Tag;

            NavigationService.Navigate(new EditReservationPage(myValue));
        }
    }
}
