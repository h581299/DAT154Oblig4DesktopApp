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
    public partial class NewReservation : Page
    {

        Dat154oblig4Context dataEntities = new Dat154oblig4Context();

        public NewReservation()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Getting data for rooms

            var roomQuery =
                from rooms in dataEntities.Rooms
                orderby rooms.Number
                select new { rooms.Number, rooms.Id };

            roomBox.ItemsSource = roomQuery.ToList();

            // Getting data for users

            var userQuery =
                from user in dataEntities.Users
                where user.Staff == false
                orderby user.Name
                select new { user.Name, user.Id };

            userBox.ItemsSource = userQuery.ToList();
        }

        private void SubmitNewReservation(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(userBox.Text)
                && !string.IsNullOrWhiteSpace(roomBox.Text)
                && (startDate.SelectedDate != null)
                && (endDate.SelectedDate != null)
            )
            {
                using (dataEntities)
                {
                    Reservation reservation = new Reservation()
                    {
                        StartDate = startDate.SelectedDate,
                        EndDate = endDate.SelectedDate,
                        RoomId = (int)roomBox.SelectedValue,
                        CustomerId = (int)userBox.SelectedValue
                    };

                    // Checking for conflicting reservations

                    var reservationQuery =
                        from reservations in dataEntities.Reservations
                        orderby reservations.Id
                        select reservations;

                    List<Reservation> reservationList = reservationQuery.ToList();

                    foreach (Reservation res in reservationList)
                    {
                        if (res.RoomId == reservation.RoomId || res.CustomerId == reservation.CustomerId)
                        {
                            if (res.StartDate <= reservation.EndDate && res.EndDate >= reservation.StartDate)
                            {
                                MessageBox.Show("Unable to add this reservation as it overlaps with an existing reservation.");
                                NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
                                return;
                            }
                        }
                    }

                    // Adding new reservation
                    dataEntities.Reservations.Add(reservation);
                    int changes = dataEntities.SaveChanges();

                    // Handling result
                    if (changes < 1)
                    {
                        MessageBox.Show("Unable to add new reservation.");
                    } else
                    {
                        MessageBox.Show("Added new reservation.");
                    }

                    NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
                }
            }
        }

        private void OnGoToReservationPageClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
        }

    }
}
