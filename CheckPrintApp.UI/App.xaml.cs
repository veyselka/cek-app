using System.Configuration;
using System.Data;
using System.Windows;
using CheckPrintApp.Core.Services;
using CheckPrintApp.UI.ViewModels;
using CheckPrintApp.UI.Services;
using Microsoft.Extensions.DependencyInjection;

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

        // Dependency Injection container'ı kur
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // Ana pencereyi aç
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
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
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}


