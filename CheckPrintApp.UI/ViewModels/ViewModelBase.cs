using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CheckPrintApp.UI.ViewModels;

/// <summary>
/// Tüm ViewModel'lar için base sınıf
/// INotifyPropertyChanged implementasyonu sağlar
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// Property değişikliği olduğunda tetiklenir
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Property değişikliğini bildirir
    /// </summary>
    /// <param name="propertyName">Değişen property adı (otomatik olarak alınır)</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Property değerini set eder ve değişikliği bildirir
    /// </summary>
    /// <typeparam name="T">Property tipi</typeparam>
    /// <param name="field">Backing field referansı</param>
    /// <param name="value">Yeni değer</param>
    /// <param name="propertyName">Property adı (otomatik olarak alınır)</param>
    /// <returns>Değer değiştiyse true</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
