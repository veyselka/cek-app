using System;
using System.Windows.Input;

namespace CheckPrintApp.UI.Helpers;

/// <summary>
/// ICommand implementasyonu - ViewModel'larda command binding için kullanılır
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    /// <summary>
    /// RelayCommand constructor
    /// </summary>
    /// <param name="execute">Komut çalıştırıldığında yapılacak aksiyon</param>
    /// <param name="canExecute">Komutun çalıştırılıp çalıştırılamayacağını belirten fonksiyon (opsiyonel)</param>
    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Parametre almayan RelayCommand constructor
    /// </summary>
    /// <param name="execute">Komut çalıştırıldığında yapılacak aksiyon</param>
    /// <param name="canExecute">Komutun çalıştırılıp çalıştırılamayacağını belirten fonksiyon (opsiyonel)</param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
        : this(
            _ => execute(),
            canExecute == null ? null : _ => canExecute())
    {
    }

    /// <summary>
    /// Komutun çalıştırılıp çalıştırılamayacağı değiştiğinde tetiklenir
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Komutun çalıştırılıp çalıştırılamayacağını kontrol eder
    /// </summary>
    /// <param name="parameter">Komut parametresi</param>
    /// <returns>Komut çalıştırılabilirse true</returns>
    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    /// <summary>
    /// Komutu çalıştırır
    /// </summary>
    /// <param name="parameter">Komut parametresi</param>
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// CanExecute durumunun yeniden değerlendirilmesini tetikler
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
