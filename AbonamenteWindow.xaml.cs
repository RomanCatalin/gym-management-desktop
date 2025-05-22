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
    /// <summary>
    /// Interaction logic for AbonamenteWindow.xaml
    /// </summary>
    public partial class AbonamenteWindow : Window
    {
        private string filePath = "C:\\Users\\Eu\\source\\repos\\PIUG\\abonamente.txt";
        private List<Abonament> abonamente = new List<Abonament>();

        public AbonamenteWindow()
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
            LoadAbonamente();


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
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void RevHomePage_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 

        }

        private void LoadAbonamente()
        {
            abonamente.Clear();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    try
                    {
                        abonamente.Add(Abonament.FromFileLine(line));
                    }
                    catch
                    {
                        // Ignori liniile corupte
                    }
                }
            }
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            EmployeeListBox.ItemsSource = null;
            EmployeeListBox.ItemsSource = abonamente;
        }

        private void CautareAbonament_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var rezultate = abonamente
                .Where(a => a.PersonName.ToLower().Contains(query) || a.Type.ToLower().Contains(query))
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

        // Opțional: metodă pentru salvarea în fișier
        private void SaveAbonamente()
        {
            File.WriteAllLines(filePath, abonamente.Select(a => a.ToFileLine()));
        }

        private void AddAbonament_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AbonamentEditWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                // Generăm un ID nou (max ID + 1)
                int newId = abonamente.Any() ? abonamente.Max(a => a.ID) + 1 : 1;
                addWindow.Abonament.ID = newId;

                abonamente.Add(addWindow.Abonament);
                SaveAbonamente();
                RefreshListBox();
            }
        }

        private void StergereAbonament_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is Abonament selected)
            {
                var result = MessageBox.Show($"Sigur doriți să ștergeți abonamentul [{selected.ID}] {selected.PersonName}?",
                    "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    abonamente.Remove(selected);
                    SaveAbonamente();
                    RefreshListBox();
                    MessageBox.Show($"Abonamentul cu ID-ul [{selected.ID}] - {selected.PersonName} a fost șters cu succes.",
                          "Ștergere reușită", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selectați un abonament pentru ștergere.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ModAbonament_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is Abonament selected)
            {
                // Creăm o copie pentru editare ca să nu modificăm direct obiectul
                var abonamentCopy = new Abonament
                {
                    ID = selected.ID,
                    PersonName = selected.PersonName,
                    Type = selected.Type,
                    StartDate = selected.StartDate,
                    EndDate = selected.EndDate
                };

                var editWindow = new AbonamentEditWindow(abonamentCopy);
                editWindow.Owner = this;

                if (editWindow.ShowDialog() == true)
                {
                    // Actualizăm obiectul original
                    selected.PersonName = editWindow.Abonament.PersonName;
                    selected.Type = editWindow.Abonament.Type;
                    selected.StartDate = editWindow.Abonament.StartDate;
                    selected.EndDate = editWindow.Abonament.EndDate;

                    SaveAbonamente();
                    RefreshListBox();
                }
            }
            else
            {
                MessageBox.Show("Selectați un abonament pentru modificare.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

}

