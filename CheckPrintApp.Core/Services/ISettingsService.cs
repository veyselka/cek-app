using CheckPrintApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckPrintApp.Core.Services;

/// <summary>
/// Uygulama ayarlarını yöneten servis interface'i
/// </summary>
public interface ISettingsService
{
    /// <summary>Ayarları dosyadan yükler</summary>
    Task<AppSettings> LoadSettingsAsync();

    /// <summary>Ayarları dosyaya kaydeder</summary>
    Task SaveSettingsAsync(AppSettings settings);

    /// <summary>Varsayılan ayarları döndürür</summary>
    AppSettings GetDefaultSettings();

    /// <summary>Ayarlar dosyasının yolunu döndürür</summary>
    string GetSettingsFilePath();

    // ─── BANKA PROFİLİ YÖNETİMİ ─────────────────────────────────────────────

    /// <summary>
    /// Mevcut kalibrasyon ayarlarını belirtilen banka adıyla kaydeder.
    /// Aynı isimde profil varsa üzerine yazar.
    /// </summary>
    Task SaveBankProfileAsync(string profileName, CalibrationConfig config);

    /// <summary>
    /// Belirtilen banka profilini siler.
    /// </summary>
    Task DeleteBankProfileAsync(string profileName);

    /// <summary>
    /// Tüm banka profillerini döndürür (Ad → Kalibrasyon).
    /// </summary>
    Task<Dictionary<string, CalibrationConfig>> GetBankProfilesAsync();
}
