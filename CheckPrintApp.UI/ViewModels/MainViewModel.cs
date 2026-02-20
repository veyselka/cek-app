using CheckPrintApp.Core.Helpers;
using CheckPrintApp.Core.Models;
using CheckPrintApp.Core.Services;
using CheckPrintApp.UI.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace CheckPrintApp.UI.ViewModels;

/// <summary>
/// Ana pencere için ViewModel
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly INumberToTextConverter _numberToTextConverter;
    private readonly ISettingsService _settingsService;
    private readonly IPrinterService _printerService;

    private CheckModel _checkModel;
    private CalibrationConfig _calibrationConfig;
    private bool _isTestPrint;
    private string _validationError = string.Empty;

    public MainViewModel(
        INumberToTextConverter numberToTextConverter,
        ISettingsService settingsService,
        IPrinterService printerService)
    {
        _numberToTextConverter = numberToTextConverter ?? throw new ArgumentNullException(nameof(numberToTextConverter));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _printerService = printerService ?? throw new ArgumentNullException(nameof(printerService));

        _checkModel = new CheckModel();
        _calibrationConfig = new CalibrationConfig();

        // Commands
        PrintCommand = new RelayCommand(async () => await ExecutePrintAsync(), CanExecutePrint);
        SaveSettingsCommand = new RelayCommand(async () => await ExecuteSaveSettingsAsync());
        ResetCalibrationCommand = new RelayCommand(ExecuteResetCalibration);

        // Ayarları yükle
        LoadSettingsAsync();
    }

    #region Properties

    /// <summary>
    /// Çek modeli
    /// </summary>
    public CheckModel CheckModel
    {
        get => _checkModel;
        set => SetProperty(ref _checkModel, value);
    }

    /// <summary>
    /// Çek tarihi
    /// </summary>
    public DateTime CheckDate
    {
        get => CheckModel.Date;
        set
        {
            if (CheckModel.Date != value)
            {
                CheckModel.Date = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedDate));
                ValidateCheck();
            }
        }
    }

    /// <summary>
    /// Alacaklı adı
    /// </summary>
    public string PayeeName
    {
        get => CheckModel.PayeeName;
        set
        {
            string processedValue = CalibrationConfig.AutoUpperCase ? value.ToUpperInvariant() : value;
            if (CheckModel.PayeeName != processedValue)
            {
                CheckModel.PayeeName = processedValue;
                OnPropertyChanged();
                ValidateCheck();
            }
        }
    }

    /// <summary>
    /// Çek tutarı
    /// </summary>
    public decimal Amount
    {
        get => CheckModel.Amount;
        set
        {
            if (CheckModel.Amount != value)
            {
                CheckModel.Amount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedAmount));
                UpdateAmountInWords();
                ValidateCheck();
            }
        }
    }

    /// <summary>
    /// Keşide yeri
    /// </summary>
    public string Location
    {
        get => CheckModel.Location;
        set
        {
            string processedValue = CalibrationConfig.AutoUpperCase ? value.ToUpperInvariant() : value;
            if (CheckModel.Location != processedValue)
            {
                CheckModel.Location = processedValue;
                OnPropertyChanged();
                ValidateCheck();
            }
        }
    }

    /// <summary>
    /// Tutarın yazıyla ifadesi
    /// </summary>
    public string AmountInWords
    {
        get => CheckModel.AmountInWords;
        private set
        {
            if (CheckModel.AmountInWords != value)
            {
                CheckModel.AmountInWords = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Formatlı tarih (önizleme için)
    /// </summary>
    public string FormattedDate => CheckModel.FormattedDate;

    /// <summary>
    /// Formatlı tutar (önizleme için)
    /// </summary>
    public string FormattedAmount => CheckModel.FormattedAmount;

    /// <summary>
    /// Kalibrasyon ayarları
    /// </summary>
    public CalibrationConfig CalibrationConfig
    {
        get => _calibrationConfig;
        set => SetProperty(ref _calibrationConfig, value);
    }

    #region Tarih Alan Pozisyonları

    public double DateOffsetX
    {
        get => CalibrationConfig.DateOffsetX;
        set
        {
            if (Math.Abs(CalibrationConfig.DateOffsetX - value) > 0.001)
            {
                CalibrationConfig.DateOffsetX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DateOffsetXText));
                OnPropertyChanged(nameof(DateOffsetXPreview));
            }
        }
    }

    public double DateOffsetY
    {
        get => CalibrationConfig.DateOffsetY;
        set
        {
            if (Math.Abs(CalibrationConfig.DateOffsetY - value) > 0.001)
            {
                CalibrationConfig.DateOffsetY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DateOffsetYText));
                OnPropertyChanged(nameof(DateOffsetYPreview));
            }
        }
    }

    public string DateOffsetXText => $"{DateOffsetX:+0.0;-0.0;0.0} mm";
    public string DateOffsetYText => $"{DateOffsetY:+0.0;-0.0;0.0} mm";
    
    // Önizleme için pixel değerleri (1mm = 3pixel önizlemede)
    public double DateOffsetXPreview => DateOffsetX * 3.0;
    public double DateOffsetYPreview => DateOffsetY * 3.0;

    #endregion

    #region Alacaklı Alan Pozisyonları

    public double PayeeOffsetX
    {
        get => CalibrationConfig.PayeeOffsetX;
        set
        {
            if (Math.Abs(CalibrationConfig.PayeeOffsetX - value) > 0.001)
            {
                CalibrationConfig.PayeeOffsetX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PayeeOffsetXText));
                OnPropertyChanged(nameof(PayeeOffsetXPreview));
            }
        }
    }

    public double PayeeOffsetY
    {
        get => CalibrationConfig.PayeeOffsetY;
        set
        {
            if (Math.Abs(CalibrationConfig.PayeeOffsetY - value) > 0.001)
            {
                CalibrationConfig.PayeeOffsetY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PayeeOffsetYText));
                OnPropertyChanged(nameof(PayeeOffsetYPreview));
            }
        }
    }

    public string PayeeOffsetXText => $"{PayeeOffsetX:+0.0;-0.0;0.0} mm";
    public string PayeeOffsetYText => $"{PayeeOffsetY:+0.0;-0.0;0.0} mm";
    
    public double PayeeOffsetXPreview => PayeeOffsetX * 3.0;
    public double PayeeOffsetYPreview => PayeeOffsetY * 3.0;

    #endregion

    #region Tutar Alan Pozisyonları

    public double AmountOffsetX
    {
        get => CalibrationConfig.AmountOffsetX;
        set
        {
            if (Math.Abs(CalibrationConfig.AmountOffsetX - value) > 0.001)
            {
                CalibrationConfig.AmountOffsetX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AmountOffsetXText));
                OnPropertyChanged(nameof(AmountOffsetXPreview));
            }
        }
    }

    public double AmountOffsetY
    {
        get => CalibrationConfig.AmountOffsetY;
        set
        {
            if (Math.Abs(CalibrationConfig.AmountOffsetY - value) > 0.001)
            {
                CalibrationConfig.AmountOffsetY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AmountOffsetYText));
                OnPropertyChanged(nameof(AmountOffsetYPreview));
            }
        }
    }

    public string AmountOffsetXText => $"{AmountOffsetX:+0.0;-0.0;0.0} mm";
    public string AmountOffsetYText => $"{AmountOffsetY:+0.0;-0.0;0.0} mm";
    
    public double AmountOffsetXPreview => AmountOffsetX * 3.0;
    public double AmountOffsetYPreview => AmountOffsetY * 3.0;

    #endregion

    #region Yazıyla Tutar Alan Pozisyonları

    public double AmountInWordsOffsetX
    {
        get => CalibrationConfig.AmountInWordsOffsetX;
        set
        {
            if (Math.Abs(CalibrationConfig.AmountInWordsOffsetX - value) > 0.001)
            {
                CalibrationConfig.AmountInWordsOffsetX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AmountInWordsOffsetXText));
                OnPropertyChanged(nameof(AmountInWordsOffsetXPreview));
            }
        }
    }

    public double AmountInWordsOffsetY
    {
        get => CalibrationConfig.AmountInWordsOffsetY;
        set
        {
            if (Math.Abs(CalibrationConfig.AmountInWordsOffsetY - value) > 0.001)
            {
                CalibrationConfig.AmountInWordsOffsetY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AmountInWordsOffsetYText));
                OnPropertyChanged(nameof(AmountInWordsOffsetYPreview));
            }
        }
    }

    public string AmountInWordsOffsetXText => $"{AmountInWordsOffsetX:+0.0;-0.0;0.0} mm";
    public string AmountInWordsOffsetYText => $"{AmountInWordsOffsetY:+0.0;-0.0;0.0} mm";
    
    public double AmountInWordsOffsetXPreview => AmountInWordsOffsetX * 3.0;
    public double AmountInWordsOffsetYPreview => AmountInWordsOffsetY * 3.0;

    #endregion

    #region Keşide Yeri Alan Pozisyonları

    public double LocationOffsetX
    {
        get => CalibrationConfig.LocationOffsetX;
        set
        {
            if (Math.Abs(CalibrationConfig.LocationOffsetX - value) > 0.001)
            {
                CalibrationConfig.LocationOffsetX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LocationOffsetXText));
                OnPropertyChanged(nameof(LocationOffsetXPreview));
            }
        }
    }

    public double LocationOffsetY
    {
        get => CalibrationConfig.LocationOffsetY;
        set
        {
            if (Math.Abs(CalibrationConfig.LocationOffsetY - value) > 0.001)
            {
                CalibrationConfig.LocationOffsetY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LocationOffsetYText));
                OnPropertyChanged(nameof(LocationOffsetYPreview));
            }
        }
    }

    public string LocationOffsetXText => $"{LocationOffsetX:+0.0;-0.0;0.0} mm";
    public string LocationOffsetYText => $"{LocationOffsetY:+0.0;-0.0;0.0} mm";
    
    public double LocationOffsetXPreview => LocationOffsetX * 3.0;
    public double LocationOffsetYPreview => LocationOffsetY * 3.0;

    #endregion

    /// <summary>
    /// Test baskısı mı?
    /// </summary>
    public bool IsTestPrint
    {
        get => _isTestPrint;
        set => SetProperty(ref _isTestPrint, value);
    }

    /// <summary>
    /// Validasyon hata mesajı
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Validasyon hatası var mı?
    /// </summary>
    public bool HasValidationError => !string.IsNullOrEmpty(ValidationError);

    #endregion

    #region Commands

    public ICommand PrintCommand { get; }
    public ICommand SaveSettingsCommand { get; }
    public ICommand ResetCalibrationCommand { get; }

    #endregion

    #region Private Methods

    /// <summary>
    /// Tutarı yazıya çevirir
    /// </summary>
    private void UpdateAmountInWords()
    {
        try
        {
            if (Amount >= ValidationHelper.MinAmount && Amount <= ValidationHelper.MaxAmount)
            {
                AmountInWords = _numberToTextConverter.Convert(Amount);
            }
            else
            {
                AmountInWords = string.Empty;
            }
        }
        catch (Exception ex)
        {
            AmountInWords = $"Hata: {ex.Message}";
        }
    }

    /// <summary>
    /// Çek bilgilerini validate eder
    /// </summary>
    private void ValidateCheck()
    {
        ValidationError = string.Empty;

        // Tutar validasyonu
        if (!ValidationHelper.ValidateAmount(Amount, out string amountError))
        {
            ValidationError = amountError;
            return;
        }

        // Alacaklı adı validasyonu
        if (!ValidationHelper.ValidatePayeeName(PayeeName, out string payeeError))
        {
            ValidationError = payeeError;
            return;
        }

        // Keşide yeri validasyonu
        if (!ValidationHelper.ValidateLocation(Location, out string locationError))
        {
            ValidationError = locationError;
            return;
        }

        // Tarih validasyonu
        if (!ValidationHelper.ValidateDate(CheckDate, out string dateError))
        {
            ValidationError = dateError;
            return;
        }

        // Tüm validasyonlar başarılı
        OnPropertyChanged(nameof(HasValidationError));
        ((RelayCommand)PrintCommand).RaiseCanExecuteChanged();
    }

    /// <summary>
    /// Ayarları yükler
    /// </summary>
    private async void LoadSettingsAsync()
    {
        try
        {
            var settings = await _settingsService.LoadSettingsAsync();

            // Kalibrasyon ayarlarını uygula
            CalibrationConfig = settings.Calibration ?? new CalibrationConfig();

            // Varsayılan keşide yerini ayarla
            if (!string.IsNullOrEmpty(settings.General?.DefaultLocation))
            {
                Location = settings.General.DefaultLocation;
            }

            // Son kullanılan çek bilgilerini yükle (opsiyonel)
            if (settings.LastCheck != null)
            {
                // İstersen son çek bilgilerini yükleyebilirsin
                // CheckDate = settings.LastCheck.Date;
                // Amount = settings.LastCheck.Amount;
            }

            // Tüm offset property'lerini güncelle
            OnPropertyChanged(nameof(DateOffsetX));
            OnPropertyChanged(nameof(DateOffsetY));
            OnPropertyChanged(nameof(DateOffsetXPreview));
            OnPropertyChanged(nameof(DateOffsetYPreview));
            OnPropertyChanged(nameof(PayeeOffsetX));
            OnPropertyChanged(nameof(PayeeOffsetY));
            OnPropertyChanged(nameof(PayeeOffsetXPreview));
            OnPropertyChanged(nameof(PayeeOffsetYPreview));
            OnPropertyChanged(nameof(AmountOffsetX));
            OnPropertyChanged(nameof(AmountOffsetY));
            OnPropertyChanged(nameof(AmountOffsetXPreview));
            OnPropertyChanged(nameof(AmountOffsetYPreview));
            OnPropertyChanged(nameof(AmountInWordsOffsetX));
            OnPropertyChanged(nameof(AmountInWordsOffsetY));
            OnPropertyChanged(nameof(AmountInWordsOffsetXPreview));
            OnPropertyChanged(nameof(AmountInWordsOffsetYPreview));
            OnPropertyChanged(nameof(LocationOffsetX));
            OnPropertyChanged(nameof(LocationOffsetY));
            OnPropertyChanged(nameof(LocationOffsetXPreview));
            OnPropertyChanged(nameof(LocationOffsetYPreview));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ayarlar yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    /// <summary>
    /// Yazdırma işlemini gerçekleştirir
    /// </summary>
    private async System.Threading.Tasks.Task ExecutePrintAsync()
    {
        try
        {
            bool success = await _printerService.PrintCheckAsync(CheckModel, CalibrationConfig, IsTestPrint);

            if (success)
            {
                MessageBox.Show("Çek başarıyla yazdırıldı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                // Son kullanılan bilgileri kaydet
                await SaveLastCheckInfoAsync();
            }
            else
            {
                MessageBox.Show("Çek yazdırılamadı. Lütfen yazıcı bağlantınızı kontrol edin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Yazdırma hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Yazdırma komutunun çalıştırılıp çalıştırılamayacağını kontrol eder
    /// </summary>
    private bool CanExecutePrint()
    {
        return !HasValidationError && _printerService.ValidatePrinterStatus();
    }

    /// <summary>
    /// Ayarları kaydeder
    /// </summary>
    private async System.Threading.Tasks.Task ExecuteSaveSettingsAsync()
    {
        try
        {
            var settings = new AppSettings
            {
                Version = "1.0.0",
                Calibration = CalibrationConfig,
                General = new GeneralSettings
                {
                    DefaultLocation = Location,
                    AutoUpperCase = CalibrationConfig.AutoUpperCase,
                    FontFamily = CalibrationConfig.FontFamily,
                    FontSize = CalibrationConfig.FontSize
                }
            };

            await _settingsService.SaveSettingsAsync(settings);
            MessageBox.Show("Ayarlar başarıyla kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ayarlar kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Son kullanılan çek bilgilerini kaydeder
    /// </summary>
    private async System.Threading.Tasks.Task SaveLastCheckInfoAsync()
    {
        try
        {
            var settings = await _settingsService.LoadSettingsAsync();
            settings.LastCheck = new LastCheckInfo
            {
                Date = CheckDate,
                PayeeName = PayeeName,
                Amount = Amount,
                Location = Location
            };
            await _settingsService.SaveSettingsAsync(settings);
        }
        catch
        {
            // Sessizce hata yut - bu kritik değil
        }
    }

    /// <summary>
    /// Kalibrasyonu sıfırlar
    /// </summary>
    private void ExecuteResetCalibration()
    {
        CalibrationConfig.Reset();
        
        // Tüm UI binding'lerini güncelle
        OnPropertyChanged(nameof(DateOffsetX));
        OnPropertyChanged(nameof(DateOffsetY));
        OnPropertyChanged(nameof(DateOffsetXPreview));
        OnPropertyChanged(nameof(DateOffsetYPreview));
        OnPropertyChanged(nameof(PayeeOffsetX));
        OnPropertyChanged(nameof(PayeeOffsetY));
        OnPropertyChanged(nameof(PayeeOffsetXPreview));
        OnPropertyChanged(nameof(PayeeOffsetYPreview));
        OnPropertyChanged(nameof(AmountOffsetX));
        OnPropertyChanged(nameof(AmountOffsetY));
        OnPropertyChanged(nameof(AmountOffsetXPreview));
        OnPropertyChanged(nameof(AmountOffsetYPreview));
        OnPropertyChanged(nameof(AmountInWordsOffsetX));
        OnPropertyChanged(nameof(AmountInWordsOffsetY));
        OnPropertyChanged(nameof(AmountInWordsOffsetXPreview));
        OnPropertyChanged(nameof(AmountInWordsOffsetYPreview));
        OnPropertyChanged(nameof(LocationOffsetX));
        OnPropertyChanged(nameof(LocationOffsetY));
        OnPropertyChanged(nameof(LocationOffsetXPreview));
        OnPropertyChanged(nameof(LocationOffsetYPreview));
        
        MessageBox.Show("Tüm kalibrasyon ayarları varsayılan değerlere döndürüldü.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    #endregion
}
