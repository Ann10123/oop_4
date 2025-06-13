using Serialization;
using System.Collections.Generic;
using System.Windows;
using System;
using Model;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace oop_4
{
    public partial class MainWindow : Window
    {
        private List<Rooms> rooms;
        public ObservableCollection<Animal> AllAnimals { get; } = new ObservableCollection<Animal>();

        public MainWindow()
        {
            InitializeComponent();
            rooms = JsonRoomStorage.Load();
            RefreshList();
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            Rooms tempRoom = new Rooms();
            bool isValid;

            do
            {
                var window = new RoomWindow(tempRoom); 
                isValid = window.ShowDialog() == true;

                if (!isValid)
                    break;

                tempRoom = window.Room;

                if (rooms.Any(r => r.Number == tempRoom.Number))
                {
                    MessageBox.Show($"Кімната №{tempRoom.Number} вже існує!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    isValid = false;
                }
            }
            while (!isValid);

            if (isValid)
            {
                rooms.Add(tempRoom);
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