using CheckPrintApp.Core.Models;
using System.Threading.Tasks;

namespace CheckPrintApp.Core.Services;

/// <summary>
/// Çek yazdırma işlemlerini yöneten servis interface'i
/// </summary>
public interface IPrinterService
{
    /// <summary>
    /// Çek yazdırır
    /// </summary>
    /// <param name="check">Yazdırılacak çek bilgileri</param>
    /// <param name="config">Kalibrasyon ayarları</param>
    /// <param name="isTestPrint">Test baskısı mı (sadece çerçeveler)?</param>
    /// <returns>Yazdırma başarılı ise true</returns>
    Task<bool> PrintCheckAsync(CheckModel check, CalibrationConfig config, bool isTestPrint);

    /// <summary>
    /// Yazıcının hazır olup olmadığını kontrol eder
    /// </summary>
    /// <returns>Yazıcı hazır ise true</returns>
    bool ValidatePrinterStatus();

    /// <summary>
    /// Kullanılabilir yazıcıları listeler
    /// </summary>
    /// <returns>Yazıcı adları koleksiyonu</returns>
    string[] GetAvailablePrinters();
}
