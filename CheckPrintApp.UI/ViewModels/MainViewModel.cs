using CheckPrintApp.Core.Helpers;
using CheckPrintApp.Core.Models;
using CheckPrintApp.Core.Services;
using CheckPrintApp.UI.Helpers;
using CheckPrintApp.UI.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Serilog;

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

    // Banka profilleri
    private ObservableCollection<string> _bankProfiles = new();
    private string? _selectedBankProfile;

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
        PrintCommand              = new RelayCommand(async () => await ExecutePrintAsync(), CanExecutePrint);
        SaveSettingsCommand       = new RelayCommand(async () => await ExecuteSaveSettingsAsync());
        ResetCalibrationCommand   = new RelayCommand(ExecuteResetCalibration);
        SaveBankProfileCommand    = new RelayCommand(async () => await ExecuteSaveBankProfileAsync());
        LoadBankProfileCommand    = new RelayCommand<string>(async (name) => await ExecuteLoadBankProfileAsync(name));
        DeleteBankProfileCommand  = new RelayCommand<string>(async (name) => await ExecuteDeleteBankProfileAsync(name));

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

    public string DateOffsetXText => $"{DateOffsetX:0.0} mm";
    public string DateOffsetYText => $"{DateOffsetY:0.0} mm";
    
    // Önizleme için pixel değerleri (Canvas 600px = 180mm çek genişliği → oran: 600/180 ≈ 3.33 px/mm)
    private const double PreviewScale = 600.0 / CalibrationConfig.CheckWidthMm; // ~3.33
    public double DateOffsetXPreview => DateOffsetX * PreviewScale;
    public double DateOffsetYPreview => DateOffsetY * PreviewScale;

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

    public string PayeeOffsetXText => $"{PayeeOffsetX:0.0} mm";
    public string PayeeOffsetYText => $"{PayeeOffsetY:0.0} mm";
    
    public double PayeeOffsetXPreview => PayeeOffsetX * PreviewScale;
    public double PayeeOffsetYPreview => PayeeOffsetY * PreviewScale;

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

    public string AmountOffsetXText => $"{AmountOffsetX:0.0} mm";
    public string AmountOffsetYText => $"{AmountOffsetY:0.0} mm";
    
    public double AmountOffsetXPreview => AmountOffsetX * PreviewScale;
    public double AmountOffsetYPreview => AmountOffsetY * PreviewScale;

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

    public string AmountInWordsOffsetXText => $"{AmountInWordsOffsetX:0.0} mm";
    public string AmountInWordsOffsetYText => $"{AmountInWordsOffsetY:0.0} mm";
    
    public double AmountInWordsOffsetXPreview => AmountInWordsOffsetX * PreviewScale;
    public double AmountInWordsOffsetYPreview => AmountInWordsOffsetY * PreviewScale;

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

    public string LocationOffsetXText => $"{LocationOffsetX:0.0} mm";
    public string LocationOffsetYText => $"{LocationOffsetY:0.0} mm";
    
    public double LocationOffsetXPreview => LocationOffsetX * PreviewScale;
    public double LocationOffsetYPreview => LocationOffsetY * PreviewScale;

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

    public ICommand PrintCommand             { get; }
    public ICommand SaveSettingsCommand      { get; }
    public ICommand ResetCalibrationCommand  { get; }
    public ICommand SaveBankProfileCommand   { get; }
    public ICommand LoadBankProfileCommand   { get; }
    public ICommand DeleteBankProfileCommand { get; }

    #endregion

    #region BankProfile Properties

    /// <summary>
    /// Kayıtlı banka profil adları listesi (UI'da görünür)
    /// </summary>
    public ObservableCollection<string> BankProfiles
    {
        get => _bankProfiles;
        private set => SetProperty(ref _bankProfiles, value);
    }

    /// <summary>
    /// Şu an seçili profil adı (yüklendikten sonra highlight için)
    /// </summary>
    public string? SelectedBankProfile
    {
        get => _selectedBankProfile;
        set => SetProperty(ref _selectedBankProfile, value);
    }

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
                Location = settings.General.DefaultLocation;

            // Banka profillerini yükle
            RefreshBankProfileList(settings.BankProfiles);

            // Tüm offset property'lerini güncelle
            NotifyAllCalibrationProperties();
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
            Log.Information("Yazdırma işlemi başlatılıyor. Alıcı: {PayeeName}, Tutar: {Amount}", PayeeName, Amount);
            
            bool success = await _printerService.PrintCheckAsync(CheckModel, CalibrationConfig, IsTestPrint);

            if (success)
            {
                Log.Information("Çek başarıyla yazdırıldı");
                MessageBox.Show("Çek başarıyla yazdırıldı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                // Son kullanılan bilgileri kaydet
                await SaveLastCheckInfoAsync();
            }
            else
            {
                Log.Warning("Çek yazdırılamadı - Yazıcı bağlantı hatası");
                MessageBox.Show("Çek yazdırılamadı. Lütfen yazıcı bağlantınızı kontrol edin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Yazdırma hatası oluştu");
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
            Log.Information("Ayarlar kaydediliyor...");

            // Mevcut ayarları yükle (profiller kaybolmasın)
            var settings = await _settingsService.LoadSettingsAsync();
            settings.Version     = "1.0.0";
            settings.Calibration = CalibrationConfig;
            settings.General     = new GeneralSettings
            {
                DefaultLocation = Location,
                AutoUpperCase   = CalibrationConfig.AutoUpperCase,
                FontFamily      = CalibrationConfig.FontFamily,
                FontSize        = CalibrationConfig.FontSize
            };

            await _settingsService.SaveSettingsAsync(settings);
            Log.Information("Ayarlar başarıyla kaydedildi");
            MessageBox.Show("Genel ayarlar başarıyla kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ayarlar kaydedilirken hata oluştu");
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
        NotifyAllCalibrationProperties();
        MessageBox.Show("Tüm kalibrasyon ayarları varsayılan değerlere döndürüldü.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void NotifyCalibrationChanged()
    {
        NotifyAllCalibrationProperties();
        Log.Information("Kalibrasyon değişiklikleri UI'a bildirildi");
    }

    // ─── BANKA PROFILİ METODLARI ─────────────────────────────────────────────────

    /// <summary>
    /// "Profil Olarak Kaydet" dialogını açar, isim alır, kaydeder.
    /// </summary>
    private async System.Threading.Tasks.Task ExecuteSaveBankProfileAsync()
    {
        try
        {
            var dialog = new SaveProfileDialog
            {
                Owner             = Application.Current.MainWindow,
                ExistingProfiles  = BankProfiles
            };

            if (dialog.ShowDialog() != true) return;

            string profileName = dialog.ProfileName;
            await _settingsService.SaveBankProfileAsync(profileName, CalibrationConfig);

            // Listeyi güncelle
            if (!BankProfiles.Contains(profileName))
            {
                BankProfiles.Add(profileName);
                // WPF Visual Tree güncellemesinin (render) tamamlanması için çok kısa bir süre bekle.
                // Aksi takdirde, RelativeSource binding'i render anında çalışırken NullReferenceException patlatabiliyor.
                await System.Threading.Tasks.Task.Delay(50);
            }

            SelectedBankProfile = profileName;

            Log.Information("Banka profili kaydedildi: {ProfileName}", profileName);
            MessageBox.Show($"✅ '{profileName}' profili kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Profil kaydedilirken hata");
            MessageBox.Show($"Profil kaydedilemedi: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Seçilen banka profilini yükler (kalibrasyon ayarlarını uygular).
    /// </summary>
    private async System.Threading.Tasks.Task ExecuteLoadBankProfileAsync(string? profileName)
    {
        if (string.IsNullOrEmpty(profileName)) return;

        try
        {
            var profiles = await _settingsService.GetBankProfilesAsync();

            if (!profiles.TryGetValue(profileName, out var config))
            {
                MessageBox.Show($"'{profileName}' profili bulunamadı.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CalibrationConfig   = config.Clone();
            SelectedBankProfile = profileName;
            NotifyAllCalibrationProperties();

            Log.Information("Banka profili yüklendi: {ProfileName}", profileName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Profil yüklenirken hata");
            MessageBox.Show($"Profil yüklenemedi: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Seçilen banka profilini siler.
    /// </summary>
    private async System.Threading.Tasks.Task ExecuteDeleteBankProfileAsync(string? profileName)
    {
        if (string.IsNullOrEmpty(profileName)) return;

        var confirm = MessageBox.Show(
            $"'{profileName}' profili silinecek. Emin misiniz?",
            "Profil Sil",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes) return;

        try
        {
            await _settingsService.DeleteBankProfileAsync(profileName);
            BankProfiles.Remove(profileName);

            if (SelectedBankProfile == profileName)
                SelectedBankProfile = null;

            Log.Information("Banka profili silindi: {ProfileName}", profileName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Profil silinirken hata");
            MessageBox.Show($"Profil silinemedi: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // ─── YARDIMCI METODLAR ──────────────────────────────────────────────────────

    /// <summary>Tüm kalibrasyon property değişikliklerini UI'a bildirir.</summary>
    private void NotifyAllCalibrationProperties()
    {
        OnPropertyChanged(nameof(DateOffsetX));             OnPropertyChanged(nameof(DateOffsetXText));
        OnPropertyChanged(nameof(DateOffsetY));             OnPropertyChanged(nameof(DateOffsetYText));
        OnPropertyChanged(nameof(DateOffsetXPreview));      OnPropertyChanged(nameof(DateOffsetYPreview));
        OnPropertyChanged(nameof(PayeeOffsetX));            OnPropertyChanged(nameof(PayeeOffsetXText));
        OnPropertyChanged(nameof(PayeeOffsetY));            OnPropertyChanged(nameof(PayeeOffsetYText));
        OnPropertyChanged(nameof(PayeeOffsetXPreview));     OnPropertyChanged(nameof(PayeeOffsetYPreview));
        OnPropertyChanged(nameof(AmountOffsetX));           OnPropertyChanged(nameof(AmountOffsetXText));
        OnPropertyChanged(nameof(AmountOffsetY));           OnPropertyChanged(nameof(AmountOffsetYText));
        OnPropertyChanged(nameof(AmountOffsetXPreview));    OnPropertyChanged(nameof(AmountOffsetYPreview));
        OnPropertyChanged(nameof(AmountInWordsOffsetX));    OnPropertyChanged(nameof(AmountInWordsOffsetXText));
        OnPropertyChanged(nameof(AmountInWordsOffsetY));    OnPropertyChanged(nameof(AmountInWordsOffsetYText));
        OnPropertyChanged(nameof(AmountInWordsOffsetXPreview)); OnPropertyChanged(nameof(AmountInWordsOffsetYPreview));
        OnPropertyChanged(nameof(LocationOffsetX));         OnPropertyChanged(nameof(LocationOffsetXText));
        OnPropertyChanged(nameof(LocationOffsetY));         OnPropertyChanged(nameof(LocationOffsetYText));
        OnPropertyChanged(nameof(LocationOffsetXPreview));  OnPropertyChanged(nameof(LocationOffsetYPreview));
    }

    /// <summary>Profil listesini Dictionary'den ObservableCollection'a dönüştürür.</summary>
    private void RefreshBankProfileList(System.Collections.Generic.Dictionary<string, CheckPrintApp.Core.Models.CalibrationConfig>? profiles)
    {
        BankProfiles.Clear();
        if (profiles == null) return;
        foreach (var key in profiles.Keys)
            BankProfiles.Add(key);
    }

    #endregion
}
