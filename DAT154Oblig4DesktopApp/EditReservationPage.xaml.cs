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
    /// Interaction logic for EditReservationPage.xaml
    /// </summary>
    public partial class EditReservationPage : Page
    {
        Dat154oblig4Context dataEntities = new Dat154oblig4Context();
        int reservationId;

        public EditReservationPage(int reservationId)
        {
            InitializeComponent();
            this.reservationId = reservationId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Getting reservation

            var reservationQuery =
                from reservations in dataEntities.Reservations
                orderby reservations.Id
                where reservations.Id == reservationId
                select reservations;

            Reservation reservation = reservationQuery.FirstOrDefault();

            // Getting data for rooms

            var roomQuery =
                from rooms in dataEntities.Rooms
                orderby rooms.Number
                select new { rooms.Number, rooms.Id };

            roomBox.ItemsSource = roomQuery.ToList();

            if (reservation != null)
            {
                if (reservation.RoomId != null)
                {
                    roomBox.SelectedValue = reservation.RoomId;
                }
            }

            // Getting data for users

            var userQuery =
                from user in dataEntities.Users
                where user.Staff == false
                orderby user.Name
                select new { user.Name, user.Id };

            userBox.ItemsSource = userQuery.ToList();

            if (reservation != null)
            {
                if (reservation.CustomerId != null)
                {
                    userBox.SelectedValue = reservation.CustomerId;
                }
            }

            if (reservation != null)
            {
                startDate.SelectedDate = reservation.StartDate;
                endDate.SelectedDate = reservation.EndDate;
            }
        }

        private void EditReservation(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(userBox.Text)
                && !string.IsNullOrWhiteSpace(roomBox.Text)
                && (startDate.SelectedDate != null)
                && (endDate.SelectedDate != null)
            )
            {
                using (dataEntities)
                {

                    var reservationQuery =
                       from reservations in dataEntities.Reservations
                       orderby reservations.Id
                       where reservations.Id == reservationId
                       select reservations;

                    Reservation reservation = reservationQuery.FirstOrDefault();
                    if (reservation != null) 
                    {
                        reservation.StartDate = startDate.SelectedDate;
                        reservation.EndDate = endDate.SelectedDate;
                        reservation.RoomId = (int)roomBox.SelectedValue;
                        reservation.CustomerId = (int)userBox.SelectedValue;

                        // Checking for conflicting reservations

                        var allReservationQuery =
                            from reservations in dataEntities.Reservations
                            orderby reservations.Id
                            select reservations;

                        List<Reservation> reservationList = allReservationQuery.ToList();

                        foreach (Reservation res in reservationList)
                        {
                            if (res.Id != reservation.Id)
                            {
                                if (res.RoomId == reservation.RoomId || res.CustomerId == reservation.CustomerId)
                                {
                                    if (res.StartDate <= reservation.EndDate && res.EndDate >= reservation.StartDate)
                                    {
                                        MessageBox.Show("Unable to modify this reservation as it overlaps with an existing reservation.");
                                        NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
                                        return;
                                    }
                                }
                            }
                        }

                        // Updating reservation
                        dataEntities.Reservations.Update(reservation);
                        int changes = dataEntities.SaveChanges();

                        // Handling results
                        if (changes < 1)
                        {
                            MessageBox.Show("Unable to modify reservation.");
                        }
                        else
                        {
                            MessageBox.Show("Modified reservation.");
                        }

                        NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
                    }
                }
            }
        }

        private void OnGoToReservationPageClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
        }
    }

}
