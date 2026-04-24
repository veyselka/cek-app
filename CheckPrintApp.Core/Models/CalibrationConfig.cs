namespace CheckPrintApp.Core.Models;

/// <summary>
/// Kalibrasyon ayarlarını temsil eden model sınıfı.
/// Her çek alanı için MUTLAK POZİSYON (mm, çekin sol-üst köşesinden itibaren) kullanılır.
/// Böylece her alan çekin herhangi bir yerine taşınabilir.
/// </summary>
public class CalibrationConfig
{
    // ─── ÇEK BOYUTLARI (mm) ─────────────────────────────────────────────────
    /// <summary>Çek genişliği (mm) - Slider maksimumu için referans</summary>
    public const double CheckWidthMm  = 180.0;
    /// <summary>Çek yüksekliği (mm) - Slider maksimumu için referans</summary>
    public const double CheckHeightMm = 80.0;

    // ─── TARİH ALANI (varsayılan: sağ üst bölge) ─────────────────────────────
    /// <summary>Tarih alanı X pozisyonu (mm) - sol kenardan</summary>
    public double DateOffsetX { get; set; } = 120;
    /// <summary>Tarih alanı Y pozisyonu (mm) - üst kenardan</summary>
    public double DateOffsetY { get; set; } = 15;

    // ─── ALACAKLI ALANI (varsayılan: sol orta bölge) ──────────────────────────
    /// <summary>Alacaklı adı X pozisyonu (mm)</summary>
    public double PayeeOffsetX { get; set; } = 20;
    /// <summary>Alacaklı adı Y pozisyonu (mm)</summary>
    public double PayeeOffsetY { get; set; } = 35;

    // ─── TUTAR ALANI (varsayılan: sağ orta bölge) ────────────────────────────
    /// <summary>Tutar (rakam) X pozisyonu (mm)</summary>
    public double AmountOffsetX { get; set; } = 120;
    /// <summary>Tutar (rakam) Y pozisyonu (mm)</summary>
    public double AmountOffsetY { get; set; } = 35;

    // ─── YAZILI TUTAR ALANI (varsayılan: alt sol bölge) ──────────────────────
    /// <summary>Yazıyla tutar X pozisyonu (mm)</summary>
    public double AmountInWordsOffsetX { get; set; } = 20;
    /// <summary>Yazıyla tutar Y pozisyonu (mm)</summary>
    public double AmountInWordsOffsetY { get; set; } = 50;

    // ─── KEŞİDE YERİ ALANI (varsayılan: sol üst bölge) ───────────────────────
    /// <summary>Keşide yeri X pozisyonu (mm)</summary>
    public double LocationOffsetX { get; set; } = 50;
    /// <summary>Keşide yeri Y pozisyonu (mm)</summary>
    public double LocationOffsetY { get; set; } = 15;

    // ─── GENEL AYARLAR ───────────────────────────────────────────────────────
    /// <summary>Varsayılan keşide yeri (şehir adı)</summary>
    public string DefaultLocation { get; set; } = "ELAZIĞ";

    /// <summary>Kullanılacak font ailesi (örn: "Arial", "Courier New")</summary>
    public string FontFamily { get; set; } = "Arial";

    /// <summary>Font boyutu (punto cinsinden)</summary>
    public double FontSize { get; set; } = 12;

    /// <summary>Otomatik büyük harf dönüşümü aktif mi?</summary>
    public bool AutoUpperCase { get; set; } = true;

    /// <summary>
    /// Kalibrasyon ayarlarını fabrika varsayılanlarına döndürür
    /// </summary>
    public void Reset()
    {
        DateOffsetX          = 120;
        DateOffsetY          = 15;
        PayeeOffsetX         = 20;
        PayeeOffsetY         = 35;
        AmountOffsetX        = 120;
        AmountOffsetY        = 35;
        AmountInWordsOffsetX = 20;
        AmountInWordsOffsetY = 50;
        LocationOffsetX      = 50;
        LocationOffsetY      = 15;
    }

    /// <summary>
    /// Kalibrasyonu kopyalar (deep copy)
    /// </summary>
    public CalibrationConfig Clone()
    {
        return new CalibrationConfig
        {
            DateOffsetX          = this.DateOffsetX,
            DateOffsetY          = this.DateOffsetY,
            PayeeOffsetX         = this.PayeeOffsetX,
            PayeeOffsetY         = this.PayeeOffsetY,
            AmountOffsetX        = this.AmountOffsetX,
            AmountOffsetY        = this.AmountOffsetY,
            AmountInWordsOffsetX = this.AmountInWordsOffsetX,
            AmountInWordsOffsetY = this.AmountInWordsOffsetY,
            LocationOffsetX      = this.LocationOffsetX,
            LocationOffsetY      = this.LocationOffsetY,
            DefaultLocation      = this.DefaultLocation,
            FontFamily           = this.FontFamily,
            FontSize             = this.FontSize,
            AutoUpperCase        = this.AutoUpperCase
        };
    }
}
