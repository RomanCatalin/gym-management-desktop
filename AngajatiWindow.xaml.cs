using System;
using System.Collections.Generic;
using System.IO;
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

namespace PIUG
{
    public partial class AngajatiWindow : Window
    {
        private string filePath = "C:\\MTP_Piug\\PIUG\\angajati.txt"; // TREBUIE MODIFICAT PC / LAPTOP !!!
        private List<Angajat> angajati = new List<Angajat>();

        public AngajatiWindow()
        {
            InitializeComponent();


            if (Properties.Settings.Default.isDarkMode)
            {
                var darkBackground = new SolidColorBrush(Color.FromRgb(44, 44, 44));
                var darkBackground2 = new SolidColorBrush(Color.FromRgb(63, 63, 63));
                var whiteForeground = Brushes.White;

                InfoBox.Foreground = whiteForeground;

                AddButton.Background = darkBackground2;
                AddButton.Foreground = whiteForeground;

                DeleteButton.Background = darkBackground2;
                DeleteButton.Foreground = whiteForeground;

                ModifyButton.Background = darkBackground2;
                ModifyButton.Foreground = whiteForeground;

                ReturnHomeButton.Background = darkBackground2;
                ReturnHomeButton.Foreground = whiteForeground;

                RightPanelBorder.Background = darkBackground;

                SearchBox.Background = darkBackground2;
                SearchBox.Foreground = whiteForeground;

                EmployeeListBox.Background = darkBackground2;
                EmployeeListBox.Foreground = whiteForeground;
            }
            else if (Properties.Settings.Default.isDarkMode == false)
            {
                var originalBackground = Brushes.White;
                var originalForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4a3762"));

                InfoBox.Foreground = originalForeground;

                AddButton.Background = originalBackground;
                AddButton.Foreground = originalForeground;

                DeleteButton.Background = originalBackground;
                DeleteButton.Foreground = originalForeground;

                ModifyButton.Background = originalBackground;
                ModifyButton.Foreground = originalForeground;

                ReturnHomeButton.Background = originalBackground;
                ReturnHomeButton.Foreground = originalForeground;

                RightPanelBorder.Background = originalBackground;

                SearchBox.Background = originalBackground;
                SearchBox.Foreground = originalForeground;

                EmployeeListBox.Background = originalBackground;
                EmployeeListBox.Foreground = originalForeground;
            }
            LoadAngajati();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.isRememberSettings == false)
            {
                Properties.Settings.Default.isDarkMode = false;
                Properties.Settings.Default.Save();
            }
            this.Close();

        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void RevHomePage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void LoadAngajati()
        {
            angajati.Clear();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    try
                    {
                        angajati.Add(Angajat.FromFileLine(line));
                    }
                    catch
                    {

                    }
                }
            }
            RefreshListBox();
        }

        private void SaveAngajati()
        {
            File.WriteAllLines(filePath, angajati.Select(a => a.ToFileLine()));
        }


        private void RefreshListBox()
        {
            EmployeeListBox.ItemsSource = null;
            EmployeeListBox.ItemsSource = angajati;
        }

        private void CautareAbonament_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var rezultate = angajati
                .Where(a => a.Nume.ToLower().Contains(query) || a.Functie.ToLower().Contains(query))
                .ToList();

            EmployeeListBox.ItemsSource = null;
            EmployeeListBox.ItemsSource = rezultate;
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                RefreshListBox();
            }
        }

        private void AddAngajat_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AngajatEditWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                int newId = angajati.Any() ? angajati.Max(a => a.ID) + 1 : 1;
                addWindow.Angajat.ID = newId;

                angajati.Add(addWindow.Angajat);
                SaveAngajati();
                RefreshListBox();
            }
        }

        private void StergereAngajat_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is Angajat selected)
            {
                var result = MessageBox.Show($"Sigur doriți să ștergeți angajatul [{selected.ID}] {selected.Nume}?",
                    "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    angajati.Remove(selected);
                    SaveAngajati();
                    RefreshListBox();
                }
            }
            else
            {
                MessageBox.Show("Selectați un angajat pentru ștergere.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ModAngajat_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is Angajat selected)
            {
                var angajatCopy = new Angajat
                {
                    ID = selected.ID,
                    Nume = selected.Nume,
                    Functie = selected.Functie,
                    Salariu = selected.Salariu
                };

                var editWindow = new AngajatEditWindow(angajatCopy);
                editWindow.Owner = this;

                if (editWindow.ShowDialog() == true)
                {
                    selected.Nume = editWindow.Angajat.Nume;
                    selected.Functie = editWindow.Angajat.Functie;
                    selected.Salariu = editWindow.Angajat.Salariu;

                    SaveAngajati();
                    RefreshListBox();
                }
            }
            else
            {
                MessageBox.Show("Selectați un angajat pentru modificare.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }

}

