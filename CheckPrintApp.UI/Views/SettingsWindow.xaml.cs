using CheckPrintApp.Core.Models;
using System.Windows;
using Serilog;

namespace CheckPrintApp.UI.Views;

/// <summary>
/// Ayarlar ve kalibrasyon penceresi
/// </summary>
public partial class SettingsWindow : Window
{
    private CalibrationConfig _config;
    private CalibrationConfig _originalConfig;

    public SettingsWindow(CalibrationConfig config)
    {
        InitializeComponent();
        _originalConfig = config;
        _config = new CalibrationConfig
        {
            DateOffsetX = config.DateOffsetX,
            DateOffsetY = config.DateOffsetY,
            PayeeOffsetX = config.PayeeOffsetX,
            PayeeOffsetY = config.PayeeOffsetY,
            AmountOffsetX = config.AmountOffsetX,
            AmountOffsetY = config.AmountOffsetY,
            AmountInWordsOffsetX = config.AmountInWordsOffsetX,
            AmountInWordsOffsetY = config.AmountInWordsOffsetY,
            LocationOffsetX = config.LocationOffsetX,
            LocationOffsetY = config.LocationOffsetY,
            FontFamily = config.FontFamily,
            FontSize = config.FontSize,
            AutoUpperCase = config.AutoUpperCase
        };

        LoadSettings();
        Log.Information("Ayarlar penceresi açıldı");
    }

    public CalibrationConfig CalibrationConfig => _config;

    private void LoadSettings()
    {
        // Genel ayarlar
        FontFamilyComboBox.Text = _config.FontFamily;
        FontSizeSlider.Value = _config.FontSize;
        AutoUpperCaseCheckBox.IsChecked = _config.AutoUpperCase;

        // Kalibrasyon ayarları
        DateXOffsetSlider.Value = _config.DateOffsetX;
        DateYOffsetSlider.Value = _config.DateOffsetY;
        
        PayeeXOffsetSlider.Value = _config.PayeeOffsetX;
        PayeeYOffsetSlider.Value = _config.PayeeOffsetY;
        
        AmountXOffsetSlider.Value = _config.AmountOffsetX;
        AmountYOffsetSlider.Value = _config.AmountOffsetY;
        
        AmountInWordsXOffsetSlider.Value = _config.AmountInWordsOffsetX;
        AmountInWordsYOffsetSlider.Value = _config.AmountInWordsOffsetY;
    }

    private void SaveSettings()
    {
        try
        {
            // Genel ayarlar
            _config.FontFamily = FontFamilyComboBox.Text;
            _config.FontSize = FontSizeSlider.Value;
            _config.AutoUpperCase = AutoUpperCaseCheckBox.IsChecked ?? false;

            // Kalibrasyon ayarları
            _config.DateOffsetX = DateXOffsetSlider.Value;
            _config.DateOffsetY = DateYOffsetSlider.Value;
            
            _config.PayeeOffsetX = PayeeXOffsetSlider.Value;
            _config.PayeeOffsetY = PayeeYOffsetSlider.Value;
            
            _config.AmountOffsetX = AmountXOffsetSlider.Value;
            _config.AmountOffsetY = AmountYOffsetSlider.Value;
            
            _config.AmountInWordsOffsetX = AmountInWordsXOffsetSlider.Value;
            _config.AmountInWordsOffsetY = AmountInWordsYOffsetSlider.Value;

            // Orijinal config'i güncelle
            _originalConfig.DateOffsetX = _config.DateOffsetX;
            _originalConfig.DateOffsetY = _config.DateOffsetY;
            _originalConfig.PayeeOffsetX = _config.PayeeOffsetX;
            _originalConfig.PayeeOffsetY = _config.PayeeOffsetY;
            _originalConfig.AmountOffsetX = _config.AmountOffsetX;
            _originalConfig.AmountOffsetY = _config.AmountOffsetY;
            _originalConfig.AmountInWordsOffsetX = _config.AmountInWordsOffsetX;
            _originalConfig.AmountInWordsOffsetY = _config.AmountInWordsOffsetY;
            _originalConfig.LocationOffsetX = _config.LocationOffsetX;
            _originalConfig.LocationOffsetY = _config.LocationOffsetY;
            _originalConfig.FontFamily = _config.FontFamily;
            _originalConfig.FontSize = _config.FontSize;
            _originalConfig.AutoUpperCase = _config.AutoUpperCase;

            Log.Information("Kalibrasyon ayarları güncellendi");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ayarlar kaydedilirken hata oluştu");
            MessageBox.Show($"Ayarlar kaydedilirken hata oluştu: {ex.Message}", "Hata", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Tüm kalibrasyon ayarlarını sıfırlamak istediğinizden emin misiniz?",
            "Sıfırla",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            DateXOffsetSlider.Value = 0;
            DateYOffsetSlider.Value = 0;
            PayeeXOffsetSlider.Value = 0;
            PayeeYOffsetSlider.Value = 0;
            AmountXOffsetSlider.Value = 0;
            AmountYOffsetSlider.Value = 0;
            AmountInWordsXOffsetSlider.Value = 0;
            AmountInWordsYOffsetSlider.Value = 0;
            
            FontFamilyComboBox.SelectedIndex = 0;
            FontSizeSlider.Value = 10;
            AutoUpperCaseCheckBox.IsChecked = false;

            Log.Information("Kalibrasyon ayarları sıfırlandı");
            MessageBox.Show("Ayarlar sıfırlandı!", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
