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
using CheckPrintApp.UI.ViewModels;
using CheckPrintApp.UI.Views;
using Serilog;

namespace CheckPrintApp.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("Ayarlar penceresi açılıyor...");
            var settingsWindow = new SettingsWindow(_viewModel.CalibrationConfig)
            {
                Owner = this
            };

            if (settingsWindow.ShowDialog() == true)
            {
                // Ayarlar kaydedildi, ViewModel'e bildir
                _viewModel.NotifyCalibrationChanged();
                MessageBox.Show("Kalibrasyon ayarları güncellendi!", "Başarılı", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Log.Information("Kalibrasyon ayarları pencereden güncellendi");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ayarlar penceresi açılırken hata oluştu");
            MessageBox.Show($"Ayarlar penceresi açılamadı: {ex.Message}", "Hata", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}