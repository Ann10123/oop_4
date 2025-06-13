using System;
using System.Windows;
using oop_4;
using Model;
using System.Linq;
using System.Windows.Controls;

namespace oop_4
{
    public partial class AccountingUnitWindow : Window
    {
        public AccountingUnit Unit { get; private set; }

        public AccountingUnitWindow()
        {
            InitializeComponent();
            BirthdayPicker.SelectedDate = DateTime.Now.AddYears(-1);
            ArrivalPicker.SelectedDate = DateTime.Now;
        }
        public AccountingUnitWindow(AccountingUnit unitToEdit)
        {
            InitializeComponent();

            NameBox.Text = unitToEdit.Animal.Name;
            ViewBox.Text = unitToEdit.Animal.View;
            CountryBox.Text = unitToEdit.Animal.Country;
            BirthdayPicker.SelectedDate = unitToEdit.Animal.Birthday;
            ArrivalPicker.SelectedDate = unitToEdit.Date;
            PriceBox.Text = unitToEdit.Price.ToString();

            Unit = unitToEdit;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameBox.Text;
                string view = ViewBox.Text;
                string country = CountryBox.Text;
                DateTime birthday = BirthdayPicker.SelectedDate ?? throw new Exception("Select birthday.");
                DateTime arrival = ArrivalPicker.SelectedDate ?? throw new Exception("Select arrival date.");
                int price = int.Parse(PriceBox.Text);

                var animal = new Animal(view, name, country, birthday);
                Unit = new AccountingUnit(animal, arrival, price);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Input error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
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
