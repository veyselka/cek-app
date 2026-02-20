using System;

namespace CheckPrintApp.Core.Models;

/// <summary>
/// Çek bilgilerini temsil eden model sınıfı
/// </summary>
public class CheckModel
{
    /// <summary>
    /// Çek tarihi
    /// </summary>
    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    /// Alacaklı (Kime/Emrine) adı
    /// </summary>
    public string PayeeName { get; set; } = string.Empty;

    /// <summary>
    /// Çek tutarı (TL cinsinden)
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Keşide yeri (şehir)
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Tutarın yazıyla ifadesi (computed property)
    /// Bu property NumberToTextConverter servisi tarafından hesaplanacak
    /// </summary>
    public string AmountInWords { get; set; } = string.Empty;

    /// <summary>
    /// Çek için formatlı tarih string (DD / MM / YYYY formatında)
    /// </summary>
    public string FormattedDate => Date.ToString("dd / MM / yyyy");

    /// <summary>
    /// Güvenlik karakterleri (#) ile çevrelenmiş tutar
    /// </summary>
    public string FormattedAmount => $"#{Amount:N2}#";

    /// <summary>
    /// Keşide yeri formatlanmış hali
    /// </summary>
    public string FormattedLocation => Location ?? string.Empty;
}
