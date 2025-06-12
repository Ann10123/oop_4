using Serialization;
using System.Collections.Generic;
using System.Windows;
using System;
using Model;
using System.Windows.Controls;

namespace oop_4
{
    public partial class MainWindow : Window
    {
        private List<Rooms> rooms;

        public MainWindow()
        {
            InitializeComponent();
            rooms = JsonRoomStorage.Load();
            RefreshList();
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var window = new RoomWindow();
            if (window.ShowDialog() == true)
            {
                rooms.Add(window.Room);
                RefreshList();
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsList.SelectedItem is Rooms selected)
            {
                var window = new RoomWindow(selected);
                if (window.ShowDialog() == true)
                    RefreshList();
            }
        }

        private void RefreshList()
        {
            RoomsList.ItemsSource = null;
            RoomsList.ItemsSource = rooms;
        }
        private void RoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Edit.IsEnabled = RoomsList.SelectedItem != null;
        }

        protected override void OnClosed(EventArgs e)
        {
            JsonRoomStorage.Save(rooms);
            base.OnClosed(e);
        }
    }
}