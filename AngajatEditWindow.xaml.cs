using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PIUG
{
    public partial class AngajatEditWindow : Window
    {
        public Angajat Angajat { get; private set; }
        private bool isEditMode;

        public AngajatEditWindow()
        {
            InitializeComponent();

            PositionComboBox.SelectedIndex = 0; 
            SalaryTextBox.Text = "0.00"; 

            if (Properties.Settings.Default.isDarkMode)
            {
                var darkBackground = new SolidColorBrush(Color.FromRgb(44, 44, 44));
                var darkBackground2 = new SolidColorBrush(Color.FromRgb(63, 63, 63));
                var whiteForeground = Brushes.White;
                SettingsBackground.Background = darkBackground;

                NameLabel.Foreground = whiteForeground;
                PositionLabel.Foreground = whiteForeground;
                SalaryLabel.Foreground = whiteForeground;

                NameTextBox.Foreground = whiteForeground;
                NameTextBox.Background = darkBackground2;

                SalaryTextBox.Foreground = whiteForeground;
                SalaryTextBox.Background = darkBackground2;
            }
            else
            {
                var originalBackground = Brushes.White;
                var originalForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4a3762"));

                SettingsBackground.Background = originalBackground;

                NameLabel.Foreground = originalForeground;
                PositionLabel.Foreground = originalForeground;
                SalaryLabel.Foreground = originalForeground;

                NameTextBox.Foreground = originalForeground;
                NameTextBox.Background = originalBackground;

                SalaryTextBox.Foreground = originalForeground;
                SalaryTextBox.Background = originalBackground;
            }
        }

        public AngajatEditWindow(Angajat angajat) : this()
        {
            if (angajat != null)
            {
                Angajat = angajat;
                isEditMode = true;

                NameTextBox.Text = angajat.Nume;

                PositionComboBox.SelectedItem = null;
                foreach (ComboBoxItem item in PositionComboBox.Items)
                {
                    if ((string)item.Content == angajat.Functie)
                    {
                        PositionComboBox.SelectedItem = item;
                        break;
                    }
                }

                SalaryTextBox.Text = angajat.Salariu.ToString("F2");
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Numele angajatului este obligatoriu.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PositionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Selectați funcția angajatului.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!double.TryParse(SalaryTextBox.Text.Trim(), out double salariu) || salariu < 0)
            {
                MessageBox.Show("Introduceți un salariu valid (număr pozitiv).", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Angajat == null)
            {
                Angajat = new Angajat();
            }

            Angajat.Nume = NameTextBox.Text.Trim();
            Angajat.Functie = ((ComboBoxItem)PositionComboBox.SelectedItem).Content.ToString();
            Angajat.Salariu = salariu;

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
