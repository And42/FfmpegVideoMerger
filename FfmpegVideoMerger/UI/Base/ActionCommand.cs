using System;
using System.Windows.Input;

namespace FfmpegVideoMerger.UI.Base;

public class ActionCommand : ICommand {

    private readonly Action<object?> _action;
    private readonly Func<object?, bool> _canExecute;

    public event EventHandler? CanExecuteChanged;

    public ActionCommand(
        Action<object?> action,
        Func<object?, bool> canExecute
    ) {
        _action = action;
        _canExecute = canExecute;
    }
    
    public ActionCommand(
        Action action,
        Func<bool> canExecute
    ) {
        _action = _ => action();
        _canExecute = _ => canExecute();
    }

    public bool CanExecute(object? parameter) {
        return _canExecute(parameter);
    }

    public void Execute(object? parameter) {
        _action(parameter);
    }

    public void RaiseCanExecuteChanged() {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}