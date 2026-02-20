using CheckPrintApp.Core.Services;
using Xunit;

namespace CheckPrintApp.Tests.Services;

public class NumberToTextConverterTests
{
    private readonly INumberToTextConverter _converter;

    public NumberToTextConverterTests()
    {
        _converter = new NumberToTextConverter();
    }

    [Fact]
    public void Convert_Zero_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 0.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("SIFIR Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_OnlyKurus_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 0.50m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("SIFIR Türk Lirası ELLİ Kuruş", result);
    }

    [Fact]
    public void Convert_OneLira_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 1.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("BİR Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_ComplexAmount_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 1250.50m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("BİN İKİ YÜZ ELLİ Türk Lirası ELLİ Kuruş", result);
    }

    [Fact]
    public void Convert_OneMillion_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 1_000_000.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("BİR MİLYON Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_MaxAmount_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 999_999_999.99m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Contains("DOKUZ YÜZ DOKSAN DOKUZ MİLYON", result);
        Assert.Contains("DOKSAN DOKUZ Kuruş", result);
    }

    [Fact]
    public void Convert_Nine_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 9.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("DOKUZ Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_Ten_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 10.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("ON Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_OneHundred_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 100.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("YÜZ Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_OneThousand_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 1000.00m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("BİN Türk Lirası SIFIR Kuruş", result);
    }

    [Fact]
    public void Convert_TwoHundredFiftyThree_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 253.75m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("İKİ YÜZ ELLİ ÜÇ Türk Lirası YETMİŞ BEŞ Kuruş", result);
    }

    [Fact]
    public void Convert_FiveThousandTwelve_ReturnsCorrectText()
    {
        // Arrange
        decimal amount = 5012.34m;

        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal("BEŞ BİN ON İKİ Türk Lirası OTUZ DÖRT Kuruş", result);
    }

    [Theory]
    [InlineData(0.01, "SIFIR Türk Lirası BİR Kuruş")]
    [InlineData(0.99, "SIFIR Türk Lirası DOKSAN DOKUZ Kuruş")]
    [InlineData(99.99, "DOKSAN DOKUZ Türk Lirası DOKSAN DOKUZ Kuruş")]
    [InlineData(999.99, "DOKUZ YÜZ DOKSAN DOKUZ Türk Lirası DOKSAN DOKUZ Kuruş")]
    public void Convert_VariousAmounts_ReturnsCorrectText(decimal amount, string expected)
    {
        // Act
        string result = _converter.Convert(amount);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_NegativeAmount_ThrowsException()
    {
        // Arrange
        decimal amount = -100.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _converter.Convert(amount));
    }

    [Fact]
    public void Convert_AmountTooLarge_ThrowsException()
    {
        // Arrange
        decimal amount = 1_000_000_000.00m; // 1 milyar (limit 999 milyon)

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _converter.Convert(amount));
    }
}
