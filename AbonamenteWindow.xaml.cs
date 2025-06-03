using Microsoft.Win32;
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
    public partial class AbonamenteWindow : Window
    {
        private List<Abonament> abonamente = new List<Abonament>();
        private string filePath;

        public AbonamenteWindow()
        {
            InitializeComponent();
            filePath = FilePaths.abonamenteFilePath;


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

                AbonamenteListBox.Background = darkBackground2;
                AbonamenteListBox.Foreground = whiteForeground;
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

                AbonamenteListBox.Background = originalBackground;
                AbonamenteListBox.Foreground = originalForeground;
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

                    }
                }
            }
            else
            {
                File.WriteAllText(filePath,string.Empty);
            }
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            AbonamenteListBox.ItemsSource = null;
            AbonamenteListBox.ItemsSource = abonamente;
        }

        private void CautareAbonament_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var rezultate = abonamente
                .Where(a => a.PersonName.ToLower().Contains(query) || a.Type.ToLower().Contains(query) || a.ID.ToString().Equals(query))
                .ToList();

            AbonamenteListBox.ItemsSource = null;
            AbonamenteListBox.ItemsSource = rezultate;
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                RefreshListBox();
            }
        }
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
                int newId = abonamente.Any() ? abonamente.Max(a => a.ID) + 1 : 1;
                addWindow.Abonament.ID = newId;

                abonamente.Add(addWindow.Abonament);
                SaveAbonamente();
                RefreshListBox();
            }
        }

        private void StergereAbonament_Click(object sender, RoutedEventArgs e)
        {
            if (AbonamenteListBox.SelectedItem is Abonament selected)
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
            if (AbonamenteListBox.SelectedItem is Abonament selected)
            {
                var abonament_aux = new Abonament
                {
                    ID = selected.ID,
                    PersonName = selected.PersonName,
                    Type = selected.Type,
                    StartDate = selected.StartDate,
                    EndDate = selected.EndDate
                };

                var editWindow = new AbonamentEditWindow(abonament_aux);
                editWindow.Owner = this;

                if (editWindow.ShowDialog() == true)
                {
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

        private void ExportAbonamente_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fișiere text (*.csv)|*.csv";
            saveFileDialog.FileName = "export_abonamente_filtrate.csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var lines = new List<string>();

                    foreach (Abonament item in AbonamenteListBox.Items)
                    {
                        lines.Add(item.ToFileLine());
                    }

                    File.WriteAllLines(saveFileDialog.FileName, lines);
                    MessageBox.Show("Export realizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la export: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

}

