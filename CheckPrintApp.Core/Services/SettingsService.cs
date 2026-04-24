using CheckPrintApp.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckPrintApp.Core.Services;

/// <summary>
/// Uygulama ayarlarını yöneten servis implementasyonu.
/// JSON dosyası kullanarak ayarları ve banka profillerini kalıcı olarak saklar.
/// </summary>
public class SettingsService : ISettingsService
{
    private const string SettingsFileName = "settings.json";
    private const string AppFolderName    = "CheckPrintMaster";

    private readonly JsonSerializerOptions _jsonOptions;

    public SettingsService()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented               = true,
            PropertyNameCaseInsensitive = true
        };
    }

    // ─── TEMEL AYAR İŞLEMLERİ ───────────────────────────────────────────────

    public async Task<AppSettings> LoadSettingsAsync()
    {
        try
        {
            string filePath = GetSettingsFilePath();
            if (!File.Exists(filePath))
                return GetDefaultSettings();

            string json     = await File.ReadAllTextAsync(filePath);
            var    settings = JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions);
            return settings ?? GetDefaultSettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ayarlar yüklenirken hata: {ex.Message}");
            return GetDefaultSettings();
        }
    }

    public async Task SaveSettingsAsync(AppSettings settings)
    {
        try
        {
            string filePath  = GetSettingsFilePath();
            string directory = Path.GetDirectoryName(filePath)!;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string json = JsonSerializer.Serialize(settings, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ayarlar kaydedilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public AppSettings GetDefaultSettings() => new AppSettings
    {
        Version = "1.0.0",
        Calibration = new CalibrationConfig
        {
            DefaultLocation = "ELAZIĞ",
            FontFamily      = "Arial",
            FontSize        = 12,
            AutoUpperCase   = true
        },
        General = new GeneralSettings
        {
            DefaultLocation = "ELAZIĞ",
            AutoUpperCase   = true,
            FontFamily      = "Arial",
            FontSize        = 12
        },
        LastCheck    = null,
        BankProfiles = new Dictionary<string, CalibrationConfig>()
    };

    public string GetSettingsFilePath()
    {
        // Portable mod: exe klasöründe "config" klasörü varsa onu kullan
        string exeDirectory      = AppDomain.CurrentDomain.BaseDirectory;
        string portableConfigDir = Path.Combine(exeDirectory, "config");

        if (Directory.Exists(portableConfigDir))
            return Path.Combine(portableConfigDir, SettingsFileName);

        // Değilse AppData/Roaming
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appDataPath, AppFolderName, SettingsFileName);
    }

    // ─── BANKA PROFİLİ YÖNETİMİ ────────────────────────────────────────────

    public async Task SaveBankProfileAsync(string profileName, CalibrationConfig config)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            throw new ArgumentException("Profil adı boş olamaz.", nameof(profileName));

        var settings = await LoadSettingsAsync();
        settings.BankProfiles ??= new Dictionary<string, CalibrationConfig>();
        settings.BankProfiles[profileName.Trim()] = config.Clone();
        await SaveSettingsAsync(settings);
    }

    public async Task DeleteBankProfileAsync(string profileName)
    {
        var settings = await LoadSettingsAsync();
        if (settings.BankProfiles?.ContainsKey(profileName) == true)
        {
            settings.BankProfiles.Remove(profileName);
            await SaveSettingsAsync(settings);
        }
    }

    public async Task<Dictionary<string, CalibrationConfig>> GetBankProfilesAsync()
    {
        var settings = await LoadSettingsAsync();
        return settings.BankProfiles ?? new Dictionary<string, CalibrationConfig>();
    }
}
