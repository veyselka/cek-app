using System.Windows;
using System.Windows.Documents;

namespace CheckPrintApp.UI.Views;

/// <summary>
/// Yazdırma önizleme penceresi
/// </summary>
public partial class PrintPreviewWindow : Window
{
    public bool ShouldPrint { get; private set; }
    
    public PrintPreviewWindow(FixedDocument document)
    {
        InitializeComponent();
        DocumentViewer.Document = document;
        ShouldPrint = false;
    }

    private void PrintButton_Click(object sender, RoutedEventArgs e)
    {
        ShouldPrint = true;
        this.DialogResult = true;
        this.Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        ShouldPrint = false;
        this.DialogResult = false;
        this.Close();
    }
}
