namespace CheckPrintApp.Core.Services;

/// <summary>
/// Sayısal değeri Türkçe yazıya çeviren servis interface'i
/// </summary>
public interface INumberToTextConverter
{
    /// <summary>
    /// Decimal tutarı Türkçe yazıya çevirir
    /// </summary>
    /// <param name="amount">Çevrilecek tutar</param>
    /// <returns>Türkçe yazıyla tutar (örn: "Yalnız BİN İKİ YÜZ ELLİ Türk Lirası ELLİ Kuruş")</returns>
    string Convert(decimal amount);
}
