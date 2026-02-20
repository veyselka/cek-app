using System.Configuration;
using System.Data;
using System.Windows;
using CheckPrintApp.Core.Services;
using CheckPrintApp.UI.ViewModels;
using CheckPrintApp.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;

namespace CheckPrintApp.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Loglama sistemini kur
        ConfigureLogging();

        Log.Information("Uygulama başlatılıyor...");

        try
        {
            // Dependency Injection container'ı kur
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Ana pencereyi aç
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            
            Log.Information("Ana pencere başarıyla açıldı");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Uygulama başlatılırken kritik hata oluştu");
            MessageBox.Show($"Uygulama başlatılamadı: {ex.Message}", "Kritik Hata", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    private void ConfigureLogging()
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CheckPrintApp",
            "Logs",
            "app-.log"
        );

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.Debug()
            .CreateLogger();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Core servisleri kaydet
        services.AddSingleton<INumberToTextConverter, NumberToTextConverter>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IPrinterService, PrinterService>();

        // ViewModels'i kaydet
        services.AddTransient<MainViewModel>();

        // Windows'u kaydet
        services.AddTransient<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("Uygulama kapatılıyor...");
        _serviceProvider?.Dispose();
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
