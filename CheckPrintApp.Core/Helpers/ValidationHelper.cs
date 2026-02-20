using System;

namespace CheckPrintApp.Core.Helpers;

/// <summary>
/// Veri validasyonu için yardımcı sınıf
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Minimum geçerli çek tutarı (0.01 TL)
    /// </summary>
    public const decimal MinAmount = 0.01m;

    /// <summary>
    /// Maksimum geçerli çek tutarı (999,999,999.99 TL)
    /// </summary>
    public const decimal MaxAmount = 999_999_999.99m;

    /// <summary>
    /// Maksimum alacaklı adı uzunluğu
    /// </summary>
    public const int MaxPayeeNameLength = 100;

    /// <summary>
    /// Maksimum keşide yeri uzunluğu
    /// </summary>
    public const int MaxLocationLength = 50;

    /// <summary>
    /// Tutar değerinin geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="amount">Kontrol edilecek tutar</param>
    /// <param name="errorMessage">Hata mesajı (geçersiz ise)</param>
    /// <returns>Geçerli ise true, aksi halde false</returns>
    public static bool ValidateAmount(decimal amount, out string errorMessage)
    {
        if (amount < MinAmount)
        {
            errorMessage = $"Tutar en az {MinAmount:N2} TL olmalıdır.";
            return false;
        }

        if (amount > MaxAmount)
        {
            errorMessage = $"Tutar en fazla {MaxAmount:N2} TL olabilir.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Tarih değerinin geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="date">Kontrol edilecek tarih</param>
    /// <param name="errorMessage">Hata mesajı (geçersiz ise)</param>
    /// <returns>Geçerli ise true, aksi halde false</returns>
    public static bool ValidateDate(DateTime date, out string errorMessage)
    {
        // Çok eski tarihlere izin verme (1900'den önce)
        if (date.Year < 1900)
        {
            errorMessage = "Geçersiz tarih. 1900'den sonraki bir tarih giriniz.";
            return false;
        }

        // Çok ileri tarihlere izin verme (100 yıl sonrasına kadar)
        if (date.Year > DateTime.Now.Year + 100)
        {
            errorMessage = "Geçersiz tarih. Çok ileri bir tarih girdiniz.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Alacaklı adının geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="payeeName">Kontrol edilecek alacaklı adı</param>
    /// <param name="errorMessage">Hata mesajı (geçersiz ise)</param>
    /// <returns>Geçerli ise true, aksi halde false</returns>
    public static bool ValidatePayeeName(string payeeName, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(payeeName))
        {
            errorMessage = "Alacaklı adı boş olamaz.";
            return false;
        }

        if (payeeName.Length > MaxPayeeNameLength)
        {
            errorMessage = $"Alacaklı adı en fazla {MaxPayeeNameLength} karakter olabilir.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Keşide yeri bilgisinin geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="location">Kontrol edilecek keşide yeri</param>
    /// <param name="errorMessage">Hata mesajı (geçersiz ise)</param>
    /// <returns>Geçerli ise true, aksi halde false</returns>
    public static bool ValidateLocation(string location, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            errorMessage = "Keşide yeri boş olamaz.";
            return false;
        }

        if (location.Length > MaxLocationLength)
        {
            errorMessage = $"Keşide yeri en fazla {MaxLocationLength} karakter olabilir.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Kalibrasyon offset değerinin geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="offset">Offset değeri (mm)</param>
    /// <param name="errorMessage">Hata mesajı (geçersiz ise)</param>
    /// <returns>Geçerli ise true, aksi halde false</returns>
    public static bool ValidateOffset(double offset, out string errorMessage)
    {
        const double MinOffset = -50.0;
        const double MaxOffset = 50.0;

        if (offset < MinOffset || offset > MaxOffset)
        {
            errorMessage = $"Offset değeri {MinOffset}mm ile {MaxOffset}mm arasında olmalıdır.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
