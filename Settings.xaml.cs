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

namespace PIUG
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            if (Properties.Settings.Default.isDarkMode)
            {
                DarkModeCheckbox.IsChecked = true;
            }
            SaveSettingsCheckbox.IsChecked = Properties.Settings.Default.isRememberSettings;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DarkModeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            SettingsBackground.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)); // dark gray
            DarkModeLabel.Foreground = Brushes.White;
            RetinereSetariLabel.Foreground = Brushes.White;

            Properties.Settings.Default.isDarkMode = true;
            Properties.Settings.Default.Save();

        }

        private void DarkModeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsBackground.Background = new LinearGradientBrush(Colors.White, Colors.White, 90);
            var defaultColor = (Color)ColorConverter.ConvertFromString("#4a3762");
            DarkModeLabel.Foreground = new SolidColorBrush(defaultColor);
            RetinereSetariLabel.Foreground = new SolidColorBrush(defaultColor);

            Properties.Settings.Default.isDarkMode = false;
            Properties.Settings.Default.Save();
        }

        private void SaveSettingsCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.isRememberSettings = true;
            Properties.Settings.Default.Save();
        }

        private void SaveSettingsCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.isRememberSettings = false;
            Properties.Settings.Default.Save();
        }
    }
}
