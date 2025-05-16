using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PIUG;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (!Properties.Settings.Default.isRememberSettings)
        {
            Properties.Settings.Default.isDarkMode = false;
            Properties.Settings.Default.Save();
        }
        if (Properties.Settings.Default.isDarkMode)
        {
            var darkBackground = new SolidColorBrush(Color.FromRgb(44, 44, 44));
            var whiteForeground = Brushes.White;
            var darkBackground2 = new SolidColorBrush(Color.FromRgb(63, 63, 63));

            UserNameLabel.Foreground = whiteForeground;

            UserNameTextBox.Foreground = whiteForeground;
            UserNameTextBox.Background = darkBackground2;


            PasswordLabel.Foreground = whiteForeground;

            PasswordBox.Foreground = whiteForeground;
            PasswordBox.Background = darkBackground2;

            BorderBackground.Background = darkBackground;
        }
        else if (Properties.Settings.Default.isDarkMode==false)
        {
            var originalBackground = Brushes.White;
            var originalForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4a3762"));
            var originalForeground2 = Brushes.Black;

            UserNameLabel.Foreground = originalForeground;

            UserNameTextBox.Foreground = originalForeground2;
            UserNameTextBox.Background = originalBackground;

            PasswordLabel.Foreground = originalForeground;

            PasswordBox.Foreground= originalForeground2;
            PasswordBox.Background= originalBackground;

            BorderBackground.Background = originalBackground;
        }
    }

    private void LoginBtn_Click(object sender, RoutedEventArgs e)
    {
        string filePath = "C:\\Users\\Eu\\source\\repos\\PIUG\\angajati.txt"; // Ajustează calea după caz
        string inputName = UserNameTextBox.Text.Trim();
        string inputPassword = PasswordBox.Password;

        // Validare: numele nu trebuie să conțină cifre
        if (inputName.Any(char.IsDigit))
        {
            MessageBox.Show("Numele nu poate conține cifre.", "Validare nume", MessageBoxButton.OK);
            return;
        }

        // Validare: parola nu trebuie să conțină spații
        if (inputPassword.Contains(" "))
        {
            MessageBox.Show("Parola nu poate conține spații.", "Validare parola", MessageBoxButton.OK);
            return;
        }

        if (!File.Exists(filePath))
        {
            MessageBox.Show("Fișierul cu angajați nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            var lines = File.ReadAllLines(filePath);

            // Căutăm dacă există angajat cu numele dat
            var angajatLinie = lines.FirstOrDefault(line =>
            {
                var parts = line.Split(';');
                return parts.Length >= 3 && parts[1].Trim().Equals(inputName, StringComparison.OrdinalIgnoreCase);
            });

            if (angajatLinie == null)
            {
                MessageBox.Show("Numele utilizatorului este incorect.", "Eroare autentificare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var partsAngajat = angajatLinie.Split(';');
            string functie = partsAngajat[2].Trim();

            if (!functie.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Nu aveți drepturi de acces. Funcția dvs. nu este de Manager.", "Eroare autentificare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (inputPassword != "admin")
            {
                MessageBox.Show("Parola este incorectă.", "Eroare autentificare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Dacă ajunge aici, toate sunt corecte
            HomeWindow homeWindow = new HomeWindow();
            homeWindow.Show();
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Eroare la procesarea fișierului: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

}