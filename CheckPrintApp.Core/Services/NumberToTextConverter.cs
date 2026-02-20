using System;
using System.Text;

namespace CheckPrintApp.Core.Services;

/// <summary>
/// Sayısal değeri Türkçe yazıya çeviren servis implementasyonu
/// </summary>
public class NumberToTextConverter : INumberToTextConverter
{
    private static readonly string[] Ones = { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
    private static readonly string[] Tens = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
    private static readonly string[] Hundreds = { "", "YÜZ", "İKİ YÜZ", "ÜÇ YÜZ", "DÖRT YÜZ", "BEŞ YÜZ", "ALTI YÜZ", "YEDİ YÜZ", "SEKİZ YÜZ", "DOKUZ YÜZ" };

    /// <summary>
    /// Decimal tutarı Türkçe yazıya çevirir
    /// </summary>
    /// <param name="amount">Çevrilecek tutar</param>
    /// <returns>Türkçe yazıyla tutar</returns>
    public string Convert(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Tutar negatif olamaz", nameof(amount));

        if (amount > 999_999_999.99m)
            throw new ArgumentException("Tutar 999.999.999,99 TL'den büyük olamaz", nameof(amount));

        // Tam ve ondalık kısımları ayır
        long integerPart = (long)Math.Floor(amount);
        int decimalPart = (int)Math.Round((amount - integerPart) * 100);

        var result = new StringBuilder();

        // Tam kısım (Lira)
        if (integerPart == 0)
        {
            result.Append("SIFIR TÜRK LİRASI");
        }
        else
        {
            result.Append(ConvertIntegerPart(integerPart));
            result.Append(" TÜRK LİRASI");
        }

        // Ondalık kısım (Kuruş)
        result.Append(" ");
        if (decimalPart == 0)
        {
            result.Append("SIFIR KRŞ");
        }
        else
        {
            result.Append(ConvertIntegerPart(decimalPart));
            result.Append(" KRŞ");
        }

        return "#" + result.ToString() + "#";
    }

    /// <summary>
    /// Tam sayı kısmını Türkçe yazıya çevirir
    /// </summary>
    private string ConvertIntegerPart(long number)
    {
        if (number == 0)
            return "SIFIR";

        var result = new StringBuilder();

        // Milyar basamağı
        if (number >= 1_000_000_000)
        {
            long billions = number / 1_000_000_000;
            if (billions == 1)
                result.Append("BİR MİLYAR ");
            else
                result.Append(ConvertThreeDigits((int)billions) + " MİLYAR ");
            number %= 1_000_000_000;
        }

        // Milyon basamağı
        if (number >= 1_000_000)
        {
            long millions = number / 1_000_000;
            if (millions == 1)
                result.Append("BİR MİLYON ");
            else
                result.Append(ConvertThreeDigits((int)millions) + " MİLYON ");
            number %= 1_000_000;
        }

        // Bin basamağı
        if (number >= 1_000)
        {
            long thousands = number / 1_000;
            if (thousands == 1)
                result.Append("BİN ");
            else
                result.Append(ConvertThreeDigits((int)thousands) + " BİN ");
            number %= 1_000;
        }

        // Son üç basamak
        if (number > 0)
        {
            result.Append(ConvertThreeDigits((int)number));
        }

        return result.ToString().Trim();
    }

    /// <summary>
    /// Üç basamaklı sayıyı Türkçe yazıya çevirir (0-999 arası)
    /// </summary>
    private string ConvertThreeDigits(int number)
    {
        if (number == 0)
            return "";

        var result = new StringBuilder();

        // Yüzler basamağı
        int hundreds = number / 100;
        if (hundreds > 0)
        {
            result.Append(Hundreds[hundreds]);
            result.Append(" ");
        }

        // Onlar basamağı
        int remainder = number % 100;
        int tens = remainder / 10;
        if (tens > 0)
        {
            result.Append(Tens[tens]);
            result.Append(" ");
        }

        // Birler basamağı
        int ones = remainder % 10;
        if (ones > 0)
        {
            result.Append(Ones[ones]);
        }

        return result.ToString().Trim();
    }
}
