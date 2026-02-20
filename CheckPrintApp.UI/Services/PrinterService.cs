using CheckPrintApp.Core.Helpers;
using CheckPrintApp.Core.Models;
using CheckPrintApp.Core.Services;
using CheckPrintApp.UI.Views;
using System;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using Serilog;

namespace CheckPrintApp.UI.Services;

/// <summary>
/// WPF tabanlı çek yazdırma servisi
/// </summary>
public class PrinterService : IPrinterService
{
    // Standart çek boyutları (örnek - gerçek çek boyutlarına göre ayarlanmalı)
    private const double CheckWidthMm = 180;  // mm
    private const double CheckHeightMm = 80;  // mm

    // Çek alanlarının base koordinatları (mm cinsinden, çekin sol üst köşesinden itibaren)
    private const double BaseDateX = 120;
    private const double BaseDateY = 15;

    private const double BasePayeeX = 20;
    private const double BasePayeeY = 35;

    private const double BaseAmountX = 120;
    private const double BaseAmountY = 35;

    private const double BaseAmountInWordsX = 20;
    private const double BaseAmountInWordsY = 50;

    private const double BaseLocationX = 50;
    private const double BaseLocationY = 15;

    /// <summary>
    /// Çek yazdırır
    /// </summary>
    public async Task<bool> PrintCheckAsync(CheckModel check, CalibrationConfig config, bool isTestPrint)
    {
        return await Task.Run(() =>
        {
            try
            {
                Log.Information("PrintCheckAsync başlatıldı. Test modu: {IsTestPrint}", isTestPrint);
                bool result = false;

                // UI thread'de çalıştırılmalı
                Application.Current.Dispatcher.Invoke(() =>
                {
                    PrintDialog printDialog = new PrintDialog();
                    
                    // Dökümanı oluştur
                    FixedDocument document = CreateCheckDocument(check, config, isTestPrint, printDialog);
                    
                    // Önizleme penceresini göster
                    PrintPreviewWindow previewWindow = new PrintPreviewWindow(document)
                    {
                        Owner = Application.Current.MainWindow
                    };
                    
                    if (previewWindow.ShowDialog() == true && previewWindow.ShouldPrint)
                    {
                        // Kullanıcıya yazıcı seçme dialogu göster
                        if (printDialog.ShowDialog() == true)
                        {
                            Log.Information("Yazdırma dialogu onaylandı. Yazıcı: {PrinterName}", printDialog.PrintQueue?.FullName);
                            // Yazdır
                            printDialog.PrintDocument(document.DocumentPaginator, "Çek Yazdırma");
                            result = true;
                        }
                        else
                        {
                            Log.Information("Kullanıcı yazdırma dialogunu iptal etti");
                        }
                    }
                    else
                    {
                        Log.Information("Kullanıcı önizleme penceresini kapattı");
                    }
                });

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "PrintCheckAsync sırasında hata oluştu");
                return false;
            }
        });
    }

    /// <summary>
    /// Yazıcının hazır olup olmadığını kontrol eder
    /// </summary>
    public bool ValidatePrinterStatus()
    {
        // Her zaman true döndür - Yazıcı dialogu zaten kullanılabilir yazıcıları gösterir
        return true;
    }

    /// <summary>
    /// Kullanılabilir yazıcıları listeler
    /// </summary>
    public string[] GetAvailablePrinters()
    {
        try
        {
            using (LocalPrintServer printServer = new LocalPrintServer())
            {
                return printServer.GetPrintQueues()
                    .Select(pq => pq.Name)
                    .ToArray();
            }
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Çek dökümanını oluşturur
    /// </summary>
    private FixedDocument CreateCheckDocument(CheckModel check, CalibrationConfig config, bool isTestPrint, PrintDialog printDialog)
    {
        // Sayfa boyutlarını ayarla
        double pageWidth = UnitConverter.MmToPixel(CheckWidthMm);
        double pageHeight = UnitConverter.MmToPixel(CheckHeightMm);

        // FixedPage oluştur
        FixedPage page = new FixedPage
        {
            Width = pageWidth,
            Height = pageHeight,
            Background = Brushes.White
        };

        // Canvas oluştur (absolute positioning için)
        Canvas canvas = new Canvas
        {
            Width = pageWidth,
            Height = pageHeight
        };

        if (isTestPrint)
        {
            // Test baskısı - sadece çerçeveler
            DrawTestFrame(canvas, config);
        }
        else
        {
            // Normal baskı - çek bilgileri
            DrawCheckContent(canvas, check, config);
        }

        page.Children.Add(canvas);

        // PageContent oluştur
        PageContent pageContent = new PageContent();
        ((IAddChild)pageContent).AddChild(page);

        // FixedDocument oluştur
        FixedDocument document = new FixedDocument();
        document.Pages.Add(pageContent);

        return document;
    }

    /// <summary>
    /// Test baskısı için çerçeveleri çizer
    /// </summary>
    private void DrawTestFrame(Canvas canvas, CalibrationConfig config)
    {
        Pen framePen = new Pen(Brushes.Blue, 1);

        // Tarih çerçevesi
        DrawRectangleWithOffset(canvas, BaseDateX, BaseDateY, 50, 10, config.DateOffsetX, config.DateOffsetY, framePen);

        // Alacaklı çerçevesi
        DrawRectangleWithOffset(canvas, BasePayeeX, BasePayeeY, 90, 10, config.PayeeOffsetX, config.PayeeOffsetY, framePen);

        // Tutar çerçevesi
        DrawRectangleWithOffset(canvas, BaseAmountX, BaseAmountY, 50, 10, config.AmountOffsetX, config.AmountOffsetY, framePen);

        // Yazıyla tutar çerçevesi
        DrawRectangleWithOffset(canvas, BaseAmountInWordsX, BaseAmountInWordsY, 150, 10, config.AmountInWordsOffsetX, config.AmountInWordsOffsetY, framePen);

        // Keşide yeri çerçevesi
        DrawRectangleWithOffset(canvas, BaseLocationX, BaseLocationY, 65, 10, config.LocationOffsetX, config.LocationOffsetY, framePen);
    }

    /// <summary>
    /// Dikdörtgen çerçeve çizer
    /// </summary>
    private void DrawRectangleWithOffset(Canvas canvas, double baseX, double baseY, double widthMm, double heightMm, double offsetX, double offsetY, Pen pen)
    {
        double x = UnitConverter.MmToPixel(baseX + offsetX);
        double y = UnitConverter.MmToPixel(baseY + offsetY);
        double width = UnitConverter.MmToPixel(widthMm);
        double height = UnitConverter.MmToPixel(heightMm);

        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle
        {
            Width = width,
            Height = height,
            Stroke = pen.Brush,
            StrokeThickness = pen.Thickness
        };

        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        canvas.Children.Add(rect);
    }

    /// <summary>
    /// Çek içeriğini çizer
    /// </summary>
    private void DrawCheckContent(Canvas canvas, CheckModel check, CalibrationConfig config)
    {
        Brush textBrush = Brushes.Black;
        double fontSize = config.FontSize;
        FontFamily fontFamily = new FontFamily(config.FontFamily);

        // Tarih
        DrawTextWithOffset(canvas, check.FormattedDate, BaseDateX, BaseDateY, config.DateOffsetX, config.DateOffsetY, fontFamily, fontSize, textBrush);

        // Alacaklı
        DrawTextWithOffset(canvas, check.PayeeName, BasePayeeX, BasePayeeY, config.PayeeOffsetX, config.PayeeOffsetY, fontFamily, fontSize, textBrush);

        // Tutar
        DrawTextWithOffset(canvas, check.FormattedAmount, BaseAmountX, BaseAmountY, config.AmountOffsetX, config.AmountOffsetY, fontFamily, fontSize, textBrush);

        // Yazıyla tutar
        DrawTextWithOffset(canvas, check.AmountInWords, BaseAmountInWordsX, BaseAmountInWordsY, config.AmountInWordsOffsetX, config.AmountInWordsOffsetY, fontFamily, fontSize, textBrush);

        // Keşide yeri
        DrawTextWithOffset(canvas, check.FormattedLocation, BaseLocationX, BaseLocationY, config.LocationOffsetX, config.LocationOffsetY, fontFamily, fontSize, textBrush);
    }

    /// <summary>
    /// Metin çizer
    /// </summary>
    private void DrawTextWithOffset(Canvas canvas, string text, double baseX, double baseY, double offsetX, double offsetY, FontFamily fontFamily, double fontSize, Brush brush)
    {
        // Kalibrasyon offset'lerini uygula
        double x = UnitConverter.MmToPixel(baseX + offsetX);
        double y = UnitConverter.MmToPixel(baseY + offsetY);

        TextBlock textBlock = new TextBlock
        {
            Text = text,
            FontFamily = fontFamily,
            FontSize = fontSize,
            Foreground = brush
        };

        Canvas.SetLeft(textBlock, x);
        Canvas.SetTop(textBlock, y);
        canvas.Children.Add(textBlock);
    }
}
