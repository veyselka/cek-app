namespace CheckPrintApp.Core.Models;

/// <summary>
/// Kalibrasyon ayarlarını temsil eden model sınıfı
/// Her çek alanı için ayrı pozisyon ayarları
/// </summary>
public class CalibrationConfig
{
    // TARİH ALANI AYARLARI
    /// <summary>
    /// Tarih alanı X ekseni offset (mm) - Sol/Sağ
    /// </summary>
    public double DateOffsetX { get; set; } = 0;

    /// <summary>
    /// Tarih alanı Y ekseni offset (mm) - Yukarı/Aşağı
    /// </summary>
    public double DateOffsetY { get; set; } = 0;

    // ALACAKLI ALANI AYARLARI
    /// <summary>
    /// Alacaklı adı X ekseni offset (mm)
    /// </summary>
    public double PayeeOffsetX { get; set; } = 0;

    /// <summary>
    /// Alacaklı adı Y ekseni offset (mm)
    /// </summary>
    public double PayeeOffsetY { get; set; } = 0;

    // TUTAR ALANI AYARLARI
    /// <summary>
    /// Tutar (rakam) X ekseni offset (mm)
    /// </summary>
    public double AmountOffsetX { get; set; } = 0;

    /// <summary>
    /// Tutar (rakam) Y ekseni offset (mm)
    /// </summary>
    public double AmountOffsetY { get; set; } = 0;

    // YAZILI TUTAR ALANI AYARLARI
    /// <summary>
    /// Yazıyla tutar X ekseni offset (mm)
    /// </summary>
    public double AmountInWordsOffsetX { get; set; } = 0;

    /// <summary>
    /// Yazıyla tutar Y ekseni offset (mm)
    /// </summary>
    public double AmountInWordsOffsetY { get; set; } = 0;

    // KEŞİDE YERİ ALANI AYARLARI
    /// <summary>
    /// Keşide yeri X ekseni offset (mm)
    /// </summary>
    public double LocationOffsetX { get; set; } = 0;

    /// <summary>
    /// Keşide yeri Y ekseni offset (mm)
    /// </summary>
    public double LocationOffsetY { get; set; } = 0;

    /// <summary>
    /// Varsayılan keşide yeri (şehir adı)
    /// </summary>
    public string DefaultLocation { get; set; } = "ELAZIĞ";

    /// <summary>
    /// Kullanılacak font ailesi (örn: "Arial", "Courier New")
    /// </summary>
    public string FontFamily { get; set; } = "Arial";

    /// <summary>
    /// Font boyutu (punto cinsinden)
    /// </summary>
    public double FontSize { get; set; } = 12;

    /// <summary>
    /// Otomatik büyük harf dönüşümü aktif mi?
    /// </summary>
    public bool AutoUpperCase { get; set; } = true;

    /// <summary>
    /// Kalibrasyon ayarlarını varsayılan değerlere döndürür
    /// </summary>
    public void Reset()
    {
        DateOffsetX = 0;
        DateOffsetY = 0;
        PayeeOffsetX = 0;
        PayeeOffsetY = 0;
        AmountOffsetX = 0;
        AmountOffsetY = 0;
        AmountInWordsOffsetX = 0;
        AmountInWordsOffsetY = 0;
        LocationOffsetX = 0;
        LocationOffsetY = 0;
    }

    /// <summary>
    /// Kalibrasyonu kopyalar (deep copy)
    /// </summary>
    public CalibrationConfig Clone()
    {
        return new CalibrationConfig
        {
            DateOffsetX = this.DateOffsetX,
            DateOffsetY = this.DateOffsetY,
            PayeeOffsetX = this.PayeeOffsetX,
            PayeeOffsetY = this.PayeeOffsetY,
            AmountOffsetX = this.AmountOffsetX,
            AmountOffsetY = this.AmountOffsetY,
            AmountInWordsOffsetX = this.AmountInWordsOffsetX,
            AmountInWordsOffsetY = this.AmountInWordsOffsetY,
            LocationOffsetX = this.LocationOffsetX,
            LocationOffsetY = this.LocationOffsetY,
            DefaultLocation = this.DefaultLocation,
            FontFamily = this.FontFamily,
            FontSize = this.FontSize,
            AutoUpperCase = this.AutoUpperCase
        };
    }
}
