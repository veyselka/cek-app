using System;

namespace CheckPrintApp.Core.Models;

/// <summary>
/// Uygulama ayarlarını kapsayan root model sınıfı (JSON serializasyon için)
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Ayarlar dosyası versiyonu
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Kalibrasyon ayarları
    /// </summary>
    public CalibrationConfig Calibration { get; set; } = new CalibrationConfig();

    /// <summary>
    /// Genel ayarlar (kalibrasyon haricindeki ayarlar)
    /// </summary>
    public GeneralSettings General { get; set; } = new GeneralSettings();

    /// <summary>
    /// Son kullanılan çek bilgileri (opsiyonel)
    /// </summary>
    public LastCheckInfo? LastCheck { get; set; }
}

/// <summary>
/// Genel uygulama ayarları
/// </summary>
public class GeneralSettings
{
    /// <summary>
    /// Varsayılan keşide yeri
    /// </summary>
    public string DefaultLocation { get; set; } = "ELAZIĞ";

    /// <summary>
    /// Otomatik büyük harf dönüşümü
    /// </summary>
    public bool AutoUpperCase { get; set; } = true;

    /// <summary>
    /// Font ailesi
    /// </summary>
    public string FontFamily { get; set; } = "Arial";

    /// <summary>
    /// Font boyutu
    /// </summary>
    public double FontSize { get; set; } = 12;
}

/// <summary>
/// Son kullanılan çek bilgileri
/// </summary>
public class LastCheckInfo
{
    /// <summary>
    /// Son kullanılan tarih
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Son kullanılan alacaklı adı
    /// </summary>
    public string PayeeName { get; set; } = string.Empty;

    /// <summary>
    /// Son kullanılan tutar
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Son kullanılan keşide yeri
    /// </summary>
    public string Location { get; set; } = string.Empty;
}
