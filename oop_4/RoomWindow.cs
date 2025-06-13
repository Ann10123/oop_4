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
        public Rooms EditedRoom => _room;
        public RoomWindow()
        {
            InitializeComponent();
            RoomTypeComboBox.ItemsSource = Enum.GetValues(typeof(RoomType));
            // Створюємо новий об'єкт Rooms з порожнім списком тварин
            _room = new Rooms();
            AnimalListBox.ItemsSource = _room.Info;
        }

        public RoomWindow(Rooms roomToEdit)
        {
            InitializeComponent();
            _room = roomToEdit;
            RoomTypeComboBox.ItemsSource = Enum.GetValues(typeof(RoomType));
            RoomTypeComboBox.SelectedItem = _room.Room;

            NumberTextBox.Text = _room.Number.ToString();
            SizeTextBox.Text = _room.Size.ToString();
            CostTextBox.Text = _room.Cost.ToString();
            AnimalListBox.ItemsSource = _room.Info;
            DataContext = _room;
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            var animalWindow = new AccountingUnitWindow();
            if (animalWindow.ShowDialog() == true)
            {
                // Додаємо тварину безпосередньо у _room.Info
                _room.Info.Add(animalWindow.Unit);

                // Оновлюємо ItemsSource, щоб перелік відобразився
                AnimalListBox.ItemsSource = null;
                AnimalListBox.ItemsSource = _room.Info;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _room.Room = (RoomType)RoomTypeComboBox.SelectedItem;
            _room.Number = int.Parse(NumberTextBox.Text);
            _room.Size = int.Parse(SizeTextBox.Text);
            _room.Cost = int.Parse(CostTextBox.Text);

            Room = _room; // Передаємо оновлений об'єкт назовні

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
