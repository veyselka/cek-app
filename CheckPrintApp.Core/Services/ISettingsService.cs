using CheckPrintApp.Core.Models;
using System.Threading.Tasks;

namespace CheckPrintApp.Core.Services;

/// <summary>
/// Uygulama ayarlarını yöneten servis interface'i
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Ayarları dosyadan yükler
    /// </summary>
    /// <returns>Yüklenen ayarlar veya varsayılan ayarlar</returns>
    Task<AppSettings> LoadSettingsAsync();

    /// <summary>
    /// Ayarları dosyaya kaydeder
    /// </summary>
    /// <param name="settings">Kaydedilecek ayarlar</param>
    Task SaveSettingsAsync(AppSettings settings);

    /// <summary>
    /// Varsayılan ayarları döndürür
    /// </summary>
    /// <returns>Varsayılan ayarlar</returns>
    AppSettings GetDefaultSettings();

    /// <summary>
    /// Ayarlar dosyasının yolunu döndürür
    /// </summary>
    /// <returns>Ayarlar dosyasının tam yolu</returns>
    string GetSettingsFilePath();
}
