using System;
using System.Windows;
using oop_4;
using Model;

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
    }
}
