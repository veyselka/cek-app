namespace CheckPrintApp.Core.Helpers;

/// <summary>
/// Birim dönüşümleri için yardımcı sınıf (milimetre-pixel çevrimi)
/// </summary>
public static class UnitConverter
{
    /// <summary>
    /// WPF'in kullandığı DPI (Device Independent Pixels)
    /// </summary>
    private const double DPI = 96.0;

    /// <summary>
    /// Bir inç'teki milimetre sayısı
    /// </summary>
    private const double MM_PER_INCH = 25.4;

    /// <summary>
    /// Milimetreyi piksele dönüştürür
    /// </summary>
    /// <param name="mm">Milimetre değeri</param>
    /// <returns>Piksel değeri</returns>
    public static double MmToPixel(double mm)
    {
        return (mm * DPI) / MM_PER_INCH;
    }

    /// <summary>
    /// Pikseli milimetreye dönüştürür
    /// </summary>
    /// <param name="pixel">Piksel değeri</param>
    /// <returns>Milimetre değeri</returns>
    public static double PixelToMm(double pixel)
    {
        return (pixel * MM_PER_INCH) / DPI;
    }

    /// <summary>
    /// Punto (pt) değerini piksele dönüştürür
    /// </summary>
    /// <param name="points">Punto değeri</param>
    /// <returns>Piksel değeri</returns>
    public static double PointsToPixels(double points)
    {
        return points * DPI / 72.0; // 1 punto = 1/72 inç
    }

    /// <summary>
    /// Pikseli punto değerine dönüştürür
    /// </summary>
    /// <param name="pixels">Piksel değeri</param>
    /// <returns>Punto değeri</returns>
    public static double PixelsToPoints(double pixels)
    {
        return pixels * 72.0 / DPI;
    }
}
