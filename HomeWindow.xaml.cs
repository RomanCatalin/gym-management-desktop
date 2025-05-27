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
    public partial class HomeWindow : Window
    {
        private int clicksAbon = 0;
        private int clicksAng = 0;
        private int clicksSetari = 0;

        private void RearanjareButoaneHomePage()
        {

            var butoane_contorizate = new List<(Button button, int clicks)>
            {
            (BTAbon, clicksAbon),(BTAng, clicksAng),(BTSetari, clicksSetari)
            };

            butoane_contorizate.Sort((a, b) => b.clicks.CompareTo(a.clicks));
            ButtonsPanel.Children.Clear();

            foreach (var (button, _) in butoane_contorizate)
            {
                ButtonsPanel.Children.Add(button);
            }
        }



        private readonly string statsFilePath = "C:\\Users\\Eu\\source\\repos\\PIUG\\stats.txt";
        public HomeWindow()
        {
            InitializeComponent();
            LoadStatistics();
            Apply_Theme();
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

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            clicksSetari++;
            RearanjareButoaneHomePage();
            Settings settings = new Settings();
            settings.ShowDialog();

            Apply_Theme();
        }

        private void AbonBtn_Click(object sender, RoutedEventArgs e)
        {
            clicksAbon++;
            RearanjareButoaneHomePage();

            AbonamenteWindow abonamenteWindow = new AbonamenteWindow();
            abonamenteWindow.Owner = this;
            this.Hide();

            abonamenteWindow.ShowDialog();

            this.Show();
            RearanjareButoaneHomePage();

        }

        private void AngBtn_Click(object sender, RoutedEventArgs e)
        {
            clicksAng++;
            RearanjareButoaneHomePage();

            AngajatiWindow angajatiWindow = new AngajatiWindow();
            angajatiWindow.Owner = this;
            this.Hide();

            angajatiWindow.ShowDialog();

            this.Show();
            RearanjareButoaneHomePage();

        }

        public void Apply_Theme()
        {
            if (Properties.Settings.Default.isDarkMode)
            {
                var darkBackground = new SolidColorBrush(Color.FromRgb(44, 44, 44));
                var darkBackground2 = new SolidColorBrush(Color.FromRgb(63, 63, 63));
                var whiteForeground = Brushes.White;
                var pForeground = new SolidColorBrush(Color.FromRgb(141, 56, 201));
                var sForeground = new SolidColorBrush(Color.FromRgb(155, 56, 201));

                BTAbon.Foreground = whiteForeground;
                BTAbon.Background = darkBackground2;

                BTAng.Foreground = whiteForeground;
                BTAng.Background = darkBackground2;

                BTSetari.Foreground = whiteForeground;
                BTSetari.Background = darkBackground2;

                BTIesire.Foreground = whiteForeground;
                BTIesire.Background = darkBackground2;

                RightBorder.Background = darkBackground;

                TextStatistice.Foreground = whiteForeground;

                R_1.Background = darkBackground2;
                R_2.Background = darkBackground2;
                R_3.Background = darkBackground2;

                TextAngajati.Foreground = whiteForeground;
                TextAbon.Foreground = whiteForeground;
                TextAbonM.Foreground = whiteForeground;

                EmployeesCountText.Foreground = sForeground;
                SubscriptionsCountText.Foreground = sForeground;
                NewSubscriptionsCountText.Foreground = sForeground;


            }
            else if (Properties.Settings.Default.isDarkMode == false)
            {
                var originalBackground = Brushes.White;
                var originalDataBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f3eaff"));
                var originalButtonForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4a3762"));
                var originalStatisticeForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5f2a87"));
                var originalTextForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4a3762"));
                var originalDataForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5f2a87"));

                BTAbon.Background = originalBackground;
                BTAbon.Foreground = originalButtonForeground;

                BTAng.Foreground = originalButtonForeground;
                BTAng.Background = originalBackground;

                BTSetari.Foreground = originalButtonForeground;
                BTSetari.Background = originalBackground;

                BTIesire.Foreground = originalButtonForeground;
                BTIesire.Background = originalBackground;

                RightBorder.Background = originalBackground;

                TextStatistice.Foreground = originalStatisticeForeground;

                R_1.Background = originalDataBackground;
                R_2.Background = originalDataBackground;
                R_3.Background = originalDataBackground;

                TextAngajati.Foreground = originalTextForeground;
                TextAbon.Foreground = originalTextForeground;
                TextAbonM.Foreground = originalTextForeground;

                EmployeesCountText.Foreground = originalDataForeground;
                SubscriptionsCountText.Foreground = originalDataForeground;
                NewSubscriptionsCountText.Foreground = originalDataForeground;

            }
        }


        private void LoadStatistics()
        {
            try
            {
                if (!File.Exists(statsFilePath))
                {
                    MessageBox.Show("Fișierul pentru statistice HomePage nu a fost gasit!");
                    return;
                }

                string employees = "0", subscriptions = "0", newThisMonth = "0";

                foreach (var line in File.ReadAllLines(statsFilePath))
                {
                    var parts = line.Split('=');
                    if (parts.Length != 2) continue;

                    switch (parts[0].Trim())
                    {
                        case "Employees":
                            employees = parts[1].Trim();
                            break;
                        case "Subscriptions":
                            subscriptions = parts[1].Trim();
                            break;
                        case "NewThisMonth":
                            newThisMonth = parts[1].Trim();
                            break;
                    }
                }

                EmployeesCountText.Text = employees;
                SubscriptionsCountText.Text = subscriptions;
                NewSubscriptionsCountText.Text = newThisMonth;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea statisticilor: " + ex.Message);
            }
        }



        private void IesireBtn_Click(object sender, RoutedEventArgs e)
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
    }
}
