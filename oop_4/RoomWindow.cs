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
using System.Windows.Shapes;
using Model;

namespace oop_4
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RoomWindow : Window
    {
        public Rooms Room { get; private set; }
        private Rooms _room;
        public RoomWindow()
        {
            InitializeComponent();

            RoomTypeComboBox.ItemsSource = Enum.GetValues(typeof(RoomType));
            RoomTypeComboBox.SelectedIndex = -1;
            NumberTextBox.Text = "";
            SizeTextBox.Text = "";
            CostTextBox.Text = "";

            _room = new Rooms();
            AnimalListBox.ItemsSource = _room.Info;

            EditAnimal.IsEnabled = false; 

            AnimalListBox.SelectionChanged += AnimalListBox_SelectionChanged; 
        }
        public RoomWindow(Rooms roomToEdit)
        {
            InitializeComponent();
            RoomTypeComboBox.ItemsSource = Enum.GetValues(typeof(RoomType));
            _room = roomToEdit ?? new Rooms();

            RoomTypeComboBox.SelectedItem = _room.Room;

            NumberTextBox.Text = _room.Number == 0 ? "" : _room.Number.ToString();
            SizeTextBox.Text = _room.Size == 0 ? "" : _room.Size.ToString();
            CostTextBox.Text = _room.Cost == 0 ? "" : _room.Cost.ToString();

            AnimalListBox.ItemsSource = _room.Info;

            EditAnimal.IsEnabled = false;

            AnimalListBox.SelectionChanged += AnimalListBox_SelectionChanged; 

            DataContext = _room;
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            var animalWindow = new AccountingUnitWindow();
            if (animalWindow.ShowDialog() == true)
            {
                _room.Info.Add(animalWindow.Unit);

                AnimalListBox.ItemsSource = null;
                AnimalListBox.ItemsSource = _room.Info;
            }
        }
        private void AnimalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditAnimal.IsEnabled = AnimalListBox.SelectedItem != null;
        }
        private void EditAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalListBox.SelectedItem is AccountingUnit selectedUnit)
            {
                var editWindow = new AccountingUnitWindow(selectedUnit); 

                if (editWindow.ShowDialog() == true)
                {

                    selectedUnit.Animal = editWindow.Unit.Animal;
                    selectedUnit.Name = editWindow.Unit.Animal.Name;
                    selectedUnit.Price = editWindow.Unit.Price;
                    selectedUnit.Date = editWindow.Unit.Date;

                    AnimalListBox.ItemsSource = null;
                    AnimalListBox.ItemsSource = _room.Info;
                }
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _room.Room = (RoomType)RoomTypeComboBox.SelectedItem;
            _room.Number = int.Parse(NumberTextBox.Text);
            _room.Size = int.Parse(SizeTextBox.Text);
            _room.Cost = int.Parse(CostTextBox.Text);

            Room = _room; 

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
