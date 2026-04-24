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
    // Çek boyutları CalibrationConfig.CheckWidthMm / CheckHeightMm üzerinden yönetilir.
    // Base koordinatlar artık CalibrationConfig'de mutlak pozisyon olarak saklanır.

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
        double pageWidth  = UnitConverter.MmToPixel(CalibrationConfig.CheckWidthMm);
        double pageHeight = UnitConverter.MmToPixel(CalibrationConfig.CheckHeightMm);

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

        // Tarih çerçevesi — pozisyon doğrudan config'den (mutlak mm)
        DrawFrameAt(canvas, config.DateOffsetX, config.DateOffsetY, 50, 10, framePen);

        // Alacaklı çerçevesi
        DrawFrameAt(canvas, config.PayeeOffsetX, config.PayeeOffsetY, 90, 10, framePen);

        // Tutar çerçevesi
        DrawFrameAt(canvas, config.AmountOffsetX, config.AmountOffsetY, 50, 10, framePen);

        // Yazıyla tutar çerçevesi
        DrawFrameAt(canvas, config.AmountInWordsOffsetX, config.AmountInWordsOffsetY, 150, 10, framePen);

        // Keşide yeri çerçevesi
        DrawFrameAt(canvas, config.LocationOffsetX, config.LocationOffsetY, 65, 10, framePen);
    }

    /// <summary>
    /// Belirtilen mutlak pozisyona (mm) dikdörtgen çerçeve çizer
    /// </summary>
    private void DrawFrameAt(Canvas canvas, double xMm, double yMm, double widthMm, double heightMm, Pen pen)
    {
        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle
        {
            Width  = UnitConverter.MmToPixel(widthMm),
            Height = UnitConverter.MmToPixel(heightMm),
            Stroke = pen.Brush,
            StrokeThickness = pen.Thickness
        };
        Canvas.SetLeft(rect, UnitConverter.MmToPixel(xMm));
        Canvas.SetTop(rect,  UnitConverter.MmToPixel(yMm));
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

        // Tarih — mutlak pozisyon (mm)
        DrawTextAt(canvas, check.FormattedDate,    config.DateOffsetX,          config.DateOffsetY,          fontFamily, fontSize, textBrush);
        DrawTextAt(canvas, check.PayeeName,         config.PayeeOffsetX,         config.PayeeOffsetY,         fontFamily, fontSize, textBrush);
        DrawTextAt(canvas, check.FormattedAmount,   config.AmountOffsetX,        config.AmountOffsetY,        fontFamily, fontSize, textBrush);
        DrawTextAt(canvas, check.AmountInWords,     config.AmountInWordsOffsetX, config.AmountInWordsOffsetY, fontFamily, fontSize, textBrush);
        DrawTextAt(canvas, check.FormattedLocation, config.LocationOffsetX,      config.LocationOffsetY,      fontFamily, fontSize, textBrush);
    }

    /// <summary>
    /// Belirtilen mutlak pozisyona (mm) metin çizer
    /// </summary>
    private void DrawTextAt(Canvas canvas, string text, double xMm, double yMm, FontFamily fontFamily, double fontSize, Brush brush)
    {
        TextBlock textBlock = new TextBlock
        {
            Text       = text,
            FontFamily = fontFamily,
            FontSize   = fontSize,
            Foreground = brush
        };
        Canvas.SetLeft(textBlock, UnitConverter.MmToPixel(xMm));
        Canvas.SetTop(textBlock,  UnitConverter.MmToPixel(yMm));
        canvas.Children.Add(textBlock);
    }
}
