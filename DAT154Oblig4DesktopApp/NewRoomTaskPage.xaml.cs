using Azure;
using DAT154Oblig4DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
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
    /// Interaction logic for NewRoomTaskPage.xaml
    /// </summary>
    public partial class NewRoomTaskPage : Page
    {
        Dat154oblig4Context dataEntities = new Dat154oblig4Context();
        int roomNumber;
        public NewRoomTaskPage(int roomNumber)
        {
            InitializeComponent();

            this.roomNumber = roomNumber;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Getting data for rooms

            var roomQuery =
                from rooms in dataEntities.Rooms
                orderby rooms.Number
                select new { rooms.Number, rooms.Id };

            roomBox.ItemsSource = roomQuery.ToList();
            roomBox.SelectedValue = roomNumber;


            // Getting data for users

            var userQuery =
                from user in dataEntities.Users
                where user.Staff == true
                orderby user.Name
                select new { user.Name, user.Id };

            userBox.ItemsSource = userQuery.ToList();
        }

        private void SubmitNewTask(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(userBox.Text)
                && !string.IsNullOrWhiteSpace(roomBox.Text)
                && !string.IsNullOrWhiteSpace(taskNote.Text)
                && !string.IsNullOrWhiteSpace(taskTypeBox.Text)
                && (taskDate.SelectedDate != null)
            )
            {
                using (dataEntities)
                {
                    Task task = new Task()
                    {
                        Date = taskDate.SelectedDate,
                        Type = taskTypeBox.Text,
                        RoomId = (int)roomBox.SelectedValue,
                        StaffId = (int)userBox.SelectedValue,
                        Note = taskNote.Text,
                        Status = "Not started"
                    };
                    dataEntities.Tasks.Add(task);
                    int changes = dataEntities.SaveChanges();

                    MessageBox.Show("Added new task.");
                    NavigationService.Navigate(new Uri("RoomPage.xaml", UriKind.Relative));
                }
            }
        }

        private void OnGoToRoomPageClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("RoomPage.xaml", UriKind.Relative));
        }
    }
}
