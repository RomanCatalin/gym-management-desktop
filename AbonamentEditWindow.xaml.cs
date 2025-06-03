using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PIUG
{
    public partial class AbonamentEditWindow : Window
    {
        public Abonament Abonament { get; private set; }
        private bool isEditMode;

        public AbonamentEditWindow()
        {
            InitializeComponent();
            TypeComboBox.SelectedIndex = 0;
            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today;
            if (Properties.Settings.Default.isDarkMode)
            {
                var darkBackground = new SolidColorBrush(Color.FromRgb(44, 44, 44));
                var darkBackground2 = new SolidColorBrush(Color.FromRgb(63, 63, 63));
                var whiteForeground = Brushes.White;
                SettingsBackground.Background = darkBackground;

                NameLabel.Foreground = whiteForeground;
                TipLabel.Foreground = whiteForeground;
                DateLabel.Foreground = whiteForeground;
                DateLabel2.Foreground = whiteForeground;

                NameTextBox.Foreground = whiteForeground;
                NameTextBox.Background = darkBackground2;

            }
            else if (Properties.Settings.Default.isDarkMode == false)
            {
                var originalBackground = Brushes.White;
                var originalForeground = new SolidColorBrush(Color.FromRgb(74, 55, 98));


                SettingsBackground.Background = originalBackground;

                NameLabel.Foreground = originalForeground;
                TipLabel.Foreground = originalForeground;
                DateLabel.Foreground = originalForeground;
                DateLabel2.Foreground = originalForeground;

                NameTextBox.Foreground = originalForeground;
                NameTextBox.Background = originalBackground;
            }
            

        }
        public AbonamentEditWindow(Abonament abonament) : this()
        {
            if (abonament != null)
            {
                Abonament = abonament;
                isEditMode = true;
                NameTextBox.Text = abonament.PersonName;
                TypeComboBox.SelectedItem = null;

                foreach (ComboBoxItem item in TypeComboBox.Items)
                {
                    if ((string)item.Content == abonament.Type)
                    {
                        TypeComboBox.SelectedItem = item;
                        break;
                    }
                }

                StartDatePicker.SelectedDate = abonament.StartDate;
                EndDatePicker.SelectedDate = abonament.EndDate;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Numele persoanei este obligatoriu.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (TypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Selectați tipul abonamentului.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Selectați data început și data sfârșit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
            {
                MessageBox.Show("Data început nu poate fi după data sfârșit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Abonament == null)
                Abonament = new Abonament();

            Abonament.PersonName = NameTextBox.Text.Trim();
            Abonament.Type = ((ComboBoxItem)TypeComboBox.SelectedItem).Content.ToString();
            Abonament.StartDate = StartDatePicker.SelectedDate.Value;
            Abonament.EndDate = EndDatePicker.SelectedDate.Value;

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }


}
