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
        private ObservableCollection<Rooms> rooms;
        public ObservableCollection<Animal> AllAnimals { get; } = new ObservableCollection<Animal>();

        public MainWindow()
        {
            InitializeComponent();
            var loadedRooms = JsonRoomStorage.Load();
            rooms = new ObservableCollection<Rooms>(loadedRooms);
            RoomsDataGrid.ItemsSource = rooms;
            Edit.IsEnabled = false;
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var tempRoom = new Rooms();

            var window = new RoomWindow(tempRoom);
            bool isValid = window.ShowDialog() == true;

            if (isValid)
            {
                if (rooms.Any(r => r.Number == tempRoom.Number))
                {
                    MessageBox.Show($"Кімната №{tempRoom.Number} вже існує!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                rooms.Add(tempRoom);
            }
        }
        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDataGrid.SelectedItem is Rooms selected)
            {
                var window = new RoomWindow(selected);
                if (window.ShowDialog() == true)
                {
                    RoomsDataGrid.Items.Refresh();
                }
            }
        }

        private void RoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Edit.IsEnabled = RoomsDataGrid.SelectedItem != null;
        }

        protected override void OnClosed(EventArgs e)
        {
            JsonRoomStorage.Save(rooms.ToList());
            base.OnClosed(e);
        }
        private bool IsTextValid(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true; 

            if (text.StartsWith(","))
                return false; 

            int commaCount = text.Count(c => c == ',');
            if (commaCount > 1)
                return false; 

            foreach (char c in text)
            {
                if (!char.IsDigit(c) && c != ',')
                    return false; 
            }

            return true;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                e.Handled = true;
                return;
            }
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            string newText = textBox.Text.Remove(selectionStart, selectionLength);
            newText = newText.Insert(selectionStart, e.Text);

            e.Handled = !IsTextValid(newText);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pasteText = (string)e.DataObject.GetData(typeof(string));
                var textBox = sender as TextBox;

                if (textBox == null)
                {
                    e.CancelCommand();
                    return;
                }

                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;

                string newText = textBox.Text.Remove(selectionStart, selectionLength);
                newText = newText.Insert(selectionStart, pasteText);

                if (!IsTextValid(newText))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

    }
}