using System;
using System.Collections.Generic;

namespace CheckPrintApp.Core.Models;

/// <summary>
/// Uygulama ayarlarını kapsayan root model sınıfı (JSON serializasyon için)
/// </summary>
public class AppSettings
{
    public string Version { get; set; } = "1.0.0";

    /// <summary>Aktif kalibrasyon ayarları (şu an kullanılan)</summary>
    public CalibrationConfig Calibration { get; set; } = new CalibrationConfig();

    /// <summary>Genel ayarlar (kalibrasyon haricindeki ayarlar)</summary>
    public GeneralSettings General { get; set; } = new GeneralSettings();

    /// <summary>Son kullanılan çek bilgileri (opsiyonel)</summary>
    public LastCheckInfo? LastCheck { get; set; }

    /// <summary>
    /// Banka profilleri: Banka Adı → Kalibrasyon Ayarları
    /// Örn: "Ziraat", "Halkbank", "İş Bankası" vb.
    /// </summary>
    public Dictionary<string, CalibrationConfig> BankProfiles { get; set; } = new();
}

/// <summary>
/// Genel uygulama ayarları
/// </summary>
public class GeneralSettings
{
    public string DefaultLocation { get; set; } = "ELAZIĞ";
    public bool   AutoUpperCase   { get; set; } = true;
    public string FontFamily      { get; set; } = "Arial";
    public double FontSize        { get; set; } = 12;
}

/// <summary>
/// Son kullanılan çek bilgileri
/// </summary>
public class LastCheckInfo
{
    public DateTime Date      { get; set; }
    public string PayeeName   { get; set; } = string.Empty;
    public decimal Amount     { get; set; }
    public string Location    { get; set; } = string.Empty;
}
