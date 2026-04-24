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

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute    = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
        : this(_ => execute(), canExecute == null ? null : _ => canExecute()) { }

    public event EventHandler? CanExecuteChanged
    {
        add    => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
    public void Execute(object? parameter)    => _execute(parameter);

    public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
}

/// <summary>
/// Tip güvenli async ICommand - string gibi belirli parametre tipleri için (profil yükleme/silme vb.)
/// </summary>
public class RelayCommand<T> : ICommand
{
    private readonly Func<T?, System.Threading.Tasks.Task> _executeAsync;
    private readonly Func<T?, bool>? _canExecute;

    public RelayCommand(Func<T?, System.Threading.Tasks.Task> executeAsync, Func<T?, bool>? canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute   = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add    => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        if (_canExecute == null) return true;
        return parameter is T t ? _canExecute(t) : _canExecute(default);
    }

    public void Execute(object? parameter)
    {
        T? typed = parameter is T t ? t : default;
        _ = _executeAsync(typed); // fire-and-forget
    }
}
