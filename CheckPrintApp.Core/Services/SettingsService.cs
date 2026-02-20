using CheckPrintApp.Core.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckPrintApp.Core.Services;

/// <summary>
/// Uygulama ayarlarını yöneten servis implementasyonu
/// JSON dosyası kullanarak ayarları kalıcı olarak saklar
/// </summary>
public class SettingsService : ISettingsService
{
    private const string SettingsFileName = "settings.json";
    private const string AppFolderName = "CheckPrintMaster";

    private readonly JsonSerializerOptions _jsonOptions;

    public SettingsService()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true, // Okunabilir JSON formatı
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Ayarları dosyadan yükler
    /// </summary>
    public async Task<AppSettings> LoadSettingsAsync()
    {
        try
        {
            string filePath = GetSettingsFilePath();

            if (!File.Exists(filePath))
            {
                // Dosya yoksa varsayılan ayarları döndür
                return GetDefaultSettings();
            }

            string json = await File.ReadAllTextAsync(filePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions);

            return settings ?? GetDefaultSettings();
        }
        catch (Exception ex)
        {
            // Hata durumunda varsayılan ayarları döndür
            Console.WriteLine($"Ayarlar yüklenirken hata: {ex.Message}");
            return GetDefaultSettings();
        }
    }

    /// <summary>
    /// Ayarları dosyaya kaydeder
    /// </summary>
    public async Task SaveSettingsAsync(AppSettings settings)
    {
        try
        {
            string filePath = GetSettingsFilePath();
            string directory = Path.GetDirectoryName(filePath)!;

            // Klasör yoksa oluştur
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(settings, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ayarlar kaydedilirken hata oluştu: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Varsayılan ayarları döndürür
    /// </summary>
    public AppSettings GetDefaultSettings()
    {
        return new AppSettings
        {
            Version = "1.0.0",
            Calibration = new CalibrationConfig
            {
                DefaultLocation = "ELAZIĞ",
                FontFamily = "Arial",
                FontSize = 12,
                AutoUpperCase = true
            },
            General = new GeneralSettings
            {
                DefaultLocation = "ELAZIĞ",
                AutoUpperCase = true,
                FontFamily = "Arial",
                FontSize = 12
            },
            LastCheck = null
        };
    }

    /// <summary>
    /// Ayarlar dosyasının yolunu döndürür
    /// </summary>
    public string GetSettingsFilePath()
    {
        // Önce portable mod kontrolü (exe ile aynı klasörde config klasörü var mı?)
        string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string portableConfigPath = Path.Combine(exeDirectory, "config", SettingsFileName);

        if (Directory.Exists(Path.Combine(exeDirectory, "config")))
        {
            return portableConfigPath;
        }

        // Portable değilse AppData/Roaming kullan
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataPath, AppFolderName);
        return Path.Combine(appFolder, SettingsFileName);
    }
}
